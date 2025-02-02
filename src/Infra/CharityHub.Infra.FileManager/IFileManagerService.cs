namespace CharityHub.Infra.FileManager;

using Microsoft.AspNetCore.Http;

public interface IFileManagerService
{
    Task<int> UploadFileAsync(byte[] fileBytes, string subDirectory, string extension);
    Task<bool> DeleteFileAsync(int fileId);
}