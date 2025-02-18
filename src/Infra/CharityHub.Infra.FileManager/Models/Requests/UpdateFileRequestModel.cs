namespace CharityHub.Infra.FileManager.Models.Requests;

public class UpdateFileRequestModel
{
    public byte[] FileBytes { get; set; }
    public string Extension { get; set; }
}