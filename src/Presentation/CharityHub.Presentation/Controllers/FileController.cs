namespace CharityHub.Presentation.Controllers;


using Infra.FileManager.Interfaces;
using Infra.FileManager.Models.Requests;

using MediatR;

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

        var result = await _fileManagerService.UploadFileAsync(new UpdateFileRequestModel
        {
            Extension = "png", FileBytes = fileBytes,
        });
        return Ok(result.IsSuccess);
    }

   
}