namespace CharityHub.Infra.FileManager.Models.Responses;

public class UpdateFileResponseModel
{
    public bool IsSuccess { get; set; } = false;
    public string FileName { get; set; }
    public string FilePath { get; set; }
}