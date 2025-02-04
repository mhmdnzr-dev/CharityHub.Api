namespace CharityHub.Infra.FileManager.Interfaces;

using CharityHub.Infra.FileManager.Models.Requests;
using CharityHub.Infra.FileManager.Models.Responses;

public interface IFileManagerService
{
    Task<UpdateFileResponseModel> UploadFileAsync(UpdateFileRequestModel requestModel);
}