namespace CharityHub.Infra.FileManager;

using Core.Application.Configuration.Models;
using Core.Domain.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using Sql.Data.DbContexts;

public class FileManagerService : IFileManagerService
{
    private readonly CharityHubCommandDbContext _context;
    private readonly string _uploadDirectory;

    public FileManagerService(CharityHubCommandDbContext context, IOptions<FileOptions> fileSettings)
    {
        _context = context;
        _uploadDirectory = fileSettings.Value.UploadDirectory;
    }

    public async Task<int> UploadFileAsync(byte[] fileBytes, string subDirectory, string extension)
    {
        if (fileBytes == null || fileBytes.Length == 0)
            throw new ArgumentException("Invalid file");


        string uploadsPath =
            Path.Combine(Directory.GetCurrentDirectory(), _uploadDirectory); // Changed to root directory

        string fileName = $"{Guid.NewGuid()}.{extension}"; // You can change the extension dynamically
        string uniqueFileName = $"{Guid.NewGuid()}_{fileName}"; // Unique file name
        string filePath = Path.Combine(uploadsPath, subDirectory, uniqueFileName);

        try
        {
            // Ensure subdirectory exists
            Directory.CreateDirectory(Path.Combine(uploadsPath, subDirectory)); // Now using the 'uploads' folder

            await File.WriteAllBytesAsync(filePath, fileBytes);

            var storedFile = new StoredFile
            {
                FileName = fileName,
                FilePath = Path.Combine(_uploadDirectory, subDirectory, uniqueFileName),
                FileType = "application/octet-stream" // Default or you can dynamically detect the MIME type
            };

            _context.StoredFiles.Add(storedFile);
            await _context.SaveChangesAsync();

            return storedFile.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating directory or saving file: {ex.Message}");
            throw new InvalidOperationException("Error uploading file", ex);
        }
    }

    public async Task<bool> DeleteFileAsync(int fileId)
    {
        var file = await _context.StoredFiles.FindAsync(fileId);
        if (file == null) return false;

        file.IsActive = false; // Soft delete
        _context.StoredFiles.Update(file);
        await _context.SaveChangesAsync();

        return true;
    }
}