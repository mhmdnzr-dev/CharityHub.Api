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
        // Path to store the uploaded file
        string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _uploadDirectory);
        string fileName = GenerateFileName(requestModel.Extension);
        string subDirectoryPath = Path.Combine(uploadsPath, requestModel.SubDirectory);
        string filePath = Path.Combine(subDirectoryPath, fileName);

        try
        {
            // Ensure the subdirectory exists, if not create it
            if (!Directory.Exists(subDirectoryPath))
            {
                _logger.LogInformation($"Creating directory: {subDirectoryPath}");
                Directory.CreateDirectory(subDirectoryPath);
            }

            // Write the file to the directory
            await File.WriteAllBytesAsync(filePath, requestModel.FileBytes);
            _logger.LogInformation($"File {fileName} uploaded to {filePath}");

            // Return response model with file details
            var result = new UpdateFileResponseModel { FileName = fileName, FilePath = filePath };
            return result;
        }
        catch (IOException ioEx)
        {
            _logger.LogError(ioEx, "I/O error occurred while uploading the file.");
            throw new InvalidOperationException("Error uploading file due to I/O issues.", ioEx);
        }
        catch (UnauthorizedAccessException uaeEx)
        {
            _logger.LogError(uaeEx, "Access denied while uploading the file.");
            throw new InvalidOperationException("Access denied while uploading file.", uaeEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred during file upload.");
            throw new InvalidOperationException("Error uploading file.", ex);
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