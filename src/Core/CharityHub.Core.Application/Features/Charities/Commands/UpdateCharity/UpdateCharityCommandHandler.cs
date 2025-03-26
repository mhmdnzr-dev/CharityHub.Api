namespace CharityHub.Core.Application.Features.Charities.Commands.UpdateCharity;

using Contract.Features.Charity.Commands;
using Contract.Features.Charity.Commands.UpdateCharity;
using Contract.Features.Charity.Queries;

using Infra.FileManager.Interfaces;
using Infra.FileManager.Models.Requests;
using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Contract.Configuration.Models;

using Primitives;

public sealed class UpdateCharityCommandHandler : CommandHandlerBase<UpdateCharityCommand>
{
    private readonly IFileManagerService _fileManagerService;
    private readonly ICharityCommandRepository _charityCommandRepository;
    private readonly ILogger<UpdateCharityCommandHandler> _logger;
    private readonly string _uploadDirectory;
    private readonly ICharityQueryRepository _charityQueryRepository;

    public UpdateCharityCommandHandler(ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        ICharityCommandRepository charityCommandRepository,
        ICharityQueryRepository charityQueryRepository,
        IFileManagerService fileManagerService,
        ILogger<UpdateCharityCommandHandler> logger,
        IOptions<FileOptions> uploadDirectory
    ) : base(
        tokenService, httpContextAccessor)
    {
        _charityCommandRepository = charityCommandRepository;
        _charityQueryRepository = charityQueryRepository;
        _fileManagerService = fileManagerService;
        _logger = logger;
        _uploadDirectory = uploadDirectory.Value.UploadDirectory;
    }

    public override async Task<int> Handle(UpdateCharityCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to handle UpdateCharityCommand for Charity Id {CharityId}.", command.Id);

        var charity = await _charityQueryRepository.GetByIdAsync(command.Id);
        if (charity == null)
        {
            _logger.LogError("Charity with Id {CharityId} not found.", command.Id);
            throw new Exception($"Charity with Id {command.Id} not found.");
        }

        _logger.LogInformation("Charity found. Patching details for Charity Id {CharityId}.", command.Id);

        charity.UpdateDetails(
            command.Name,
            command.Description,
            command.Website,
            command.Address,
            command.CityId,
            command.Telephone,
            command.ManagerName,
            command.ContactName,
            command.ContactPhone
        );

        if (command.BannerBase64 != null && !string.IsNullOrWhiteSpace(command.BannerFileExtension))
        {
            _logger.LogInformation("Uploading new banner for Charity Id {CharityId}.", command.Id);

            var bannerFileServiceResponse = await _fileManagerService.UploadFileAsync(new UpdateFileRequestModel
            {
                Extension = command.BannerFileExtension, FileBytes = command.BannerBase64
            });

            _logger.LogInformation("Banner uploaded successfully. FileName: {FileName}",
                bannerFileServiceResponse.FileName);
            charity.SetBanner(
                bannerFileServiceResponse.FileName,
                Path.Combine(_uploadDirectory, bannerFileServiceResponse.FileName),
                "application/octet-stream"
            );
        }

        if (command.LogoBase64 != null && !string.IsNullOrWhiteSpace(command.LogoFileExtension))
        {
            _logger.LogInformation("Uploading new logo for Charity Id {CharityId}.", command.Id);

            var logoFileServiceResponse = await _fileManagerService.UploadFileAsync(new UpdateFileRequestModel
            {
                Extension = command.LogoFileExtension, FileBytes = command.LogoBase64
            });

            _logger.LogInformation("Logo uploaded successfully. FileName: {FileName}",
                logoFileServiceResponse.FileName);
            charity.SetLogo(
                logoFileServiceResponse.FileName,
                Path.Combine(_uploadDirectory, logoFileServiceResponse.FileName),
                "application/octet-stream"
            );
        }

        _logger.LogInformation("Updating Charity Id {CharityId} in the repository.", command.Id);
        await _charityCommandRepository.UpdateAsync(charity);

        _logger.LogInformation("Charity Id {CharityId} updated successfully.", command.Id);
        return charity.Id;
    }
}