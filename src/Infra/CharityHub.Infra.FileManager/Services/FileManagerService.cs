namespace CharityHub.Infra.FileManager.Services;

using System.Text;


using CharityHub.Infra.FileManager.Interfaces;
using CharityHub.Infra.FileManager.Models.Requests;
using CharityHub.Infra.FileManager.Models.Responses;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CharityHub.Core.Contract.Configuration.Models;

public sealed class FileManagerService : IFileManagerService
{
    private readonly ILogger<FileManagerService> _logger;
    private readonly string _uploadDirectory;

    public FileManagerService(ILogger<FileManagerService> logger, IOptions<FileOptions> fileSettings)
    {
        _logger = logger;
        _uploadDirectory = fileSettings.Value.UploadDirectory;
    }

    public async Task<UpdateFileResponseModel> UploadFileAsync(UpdateFileRequestModel requestModel)
    {
        string uploadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            _uploadDirectory);
        string subDirectoryPath = Path.Combine(uploadsPath, requestModel.SubDirectory);
        string fileName = GenerateFileName(requestModel.Extension);
        string filePath = Path.Combine(subDirectoryPath, fileName);

        try
        {
            // Ensure that the uploads directory exists and has write permissions
            if (!Directory.Exists(subDirectoryPath))
            {
                _logger.LogInformation("Creating directory: {SubDirectoryPath}", subDirectoryPath);
                Directory.CreateDirectory(subDirectoryPath); // Will throw an exception if it's read-only
            }

            // Ensure the file system is not in a read-only state before attempting to write
            if ((new FileInfo(subDirectoryPath).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                _logger.LogError("Directory {SubDirectoryPath} is read-only.", subDirectoryPath);
                throw new InvalidOperationException("The file system or directory is in a read-only state.");
            }

            // Write the file to the server
            await File.WriteAllBytesAsync(filePath, requestModel.FileBytes);

            _logger.LogInformation("File {FileName} uploaded successfully to {FilePath}", fileName, filePath);

            var result = new UpdateFileResponseModel { FileName = fileName, FilePath = filePath };

            return result;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied while uploading file.");
            throw new InvalidOperationException("Access denied while uploading file.", ex);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "I/O error while uploading file.");
            throw new InvalidOperationException("I/O error while uploading file.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while uploading file.");
            throw new InvalidOperationException("Unexpected error while uploading file.", ex);
        }
    }


    private string GenerateFileName(string extension)
    {
        var fileNameBuilder = new StringBuilder();
        fileNameBuilder.Append(DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"))
            .Append('_')
            .Append(Guid.CreateVersion7())
            .Append('.')
            .Append(extension);

        return fileNameBuilder.ToString();
    }
}