namespace CharityHub.Presentation.Controllers;

using Infra.FileManager;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class FileController : BaseController
{
    private readonly IFileManagerService _fileManagerService;

    public FileController(IMediator mediator, IFileManagerService fileManagerService) : base(mediator)
    {
        _fileManagerService = fileManagerService;
    }


    [MapToApiVersion("1.0")]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] string base64File)
    {
        if (string.IsNullOrEmpty(base64File))
            return BadRequest("No file provided");

        var fileBytes = Convert.FromBase64String(base64File);

        int fileId = await _fileManagerService.UploadFileAsync(fileBytes, "charities", "png");
        return Ok(new { FileId = fileId });
    }

    [HttpDelete("{fileId}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        bool result = await _fileManagerService.DeleteFileAsync(fileId);
        return result ? Ok("File deleted") : NotFound("File not found");
    }
}