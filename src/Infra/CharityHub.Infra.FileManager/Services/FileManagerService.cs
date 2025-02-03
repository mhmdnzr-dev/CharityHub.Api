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
        string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _uploadDirectory);
        string fileName = GenerateFileName(requestModel.Extension);
        string filePath = Path.Combine(uploadsPath, requestModel.SubDirectory, fileName);

        try
        {
            Directory.CreateDirectory(Path.Combine(uploadsPath, requestModel.SubDirectory));

            await File.WriteAllBytesAsync(filePath, requestModel.FileBytes);

            _logger.LogInformation($"File {fileName} uploaded to {uploadsPath}");

            var result = new UpdateFileResponseModel { FileName = fileName, FilePath = filePath };
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating directory or saving file: {ex.Message}");
            _logger.LogError(ex, "Error creating directory or saving file");
            throw new InvalidOperationException("Error uploading file", ex);
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