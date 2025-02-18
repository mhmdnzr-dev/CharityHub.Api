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
        try
        {
            // Use a platform-friendly upload directory
            string baseUploadPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "uploads");

            // Get UploadDirectory from appsettings.json
            var uploadDirectory = _uploadDirectory;
            uploadDirectory = uploadDirectory.TrimStart('/');

            string fileName = GenerateFileName(requestModel.Extension);
            string filePath = Path.Combine(uploadDirectory, fileName);


            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory);


            // Ensure the directory exists and is writable
            EnsureDirectoryWritable(uploadsPath);

            // Write file to disk
            await File.WriteAllBytesAsync(filePath, requestModel.FileBytes);

            _logger.LogInformation("File {FileName} uploaded successfully to {FilePath}", fileName, filePath);

            return new UpdateFileResponseModel { FileName = fileName, FilePath = filePath };
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

    /// <summary>
    /// Ensures the given directory exists and is writable.
    /// </summary>
    private void EnsureDirectoryWritable(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            _logger.LogInformation("Creating directory: {DirectoryPath}", directoryPath);
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            // Test if the directory is writable
            string testFile = Path.Combine(directoryPath, "testfile.tmp");
            File.WriteAllText(testFile, "test");
            File.Delete(testFile);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogError("Directory {DirectoryPath} is not writable. Check permissions.", directoryPath);
            throw new InvalidOperationException($"The directory {directoryPath} is not writable. Check permissions.");
        }
        catch (IOException)
        {
            _logger.LogError("Directory {DirectoryPath} is in a read-only file system.", directoryPath);
            throw new InvalidOperationException($"The directory {directoryPath} is in a read-only file system.");
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