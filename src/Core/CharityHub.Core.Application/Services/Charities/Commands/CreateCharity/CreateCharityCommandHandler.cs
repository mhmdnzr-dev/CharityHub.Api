namespace CharityHub.Core.Application.Services.Charities.Commands.CreateCharity;

using Contract.Charity.Commands.CreateCharity;
using Contract.Primitives.Handlers;

using Domain.Entities;

using CharityHub.Infra.FileManager.Interfaces;

using Contract.Charity.Commands;
using Contract.Configuration.Models;
using Contract.Primitives.Repositories;

using Infra.FileManager.Models.Requests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class CreateCharityCommandHandler : ICommandHandler<CreateCharityCommand>
{
    private readonly IFileManagerService _fileManagerService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICharityCommandRepository _charityCommandRepository;
    private readonly ILogger<CreateCharityCommandHandler> _logger;
    private readonly string _uploadDirectory;

    public CreateCharityCommandHandler(IFileManagerService fileManagerService, IUnitOfWork unitOfWork,
        ICharityCommandRepository charityCommandRepository, ILogger<CreateCharityCommandHandler> logger,
        IOptions<FileOptions> fileSettings)
    {
        _fileManagerService = fileManagerService;
        _unitOfWork = unitOfWork;
        _charityCommandRepository = charityCommandRepository;
        _logger = logger;

        _uploadDirectory = fileSettings.Value.UploadDirectory;
    }

    public async Task<int> Handle(CreateCharityCommand command, CancellationToken cancellationToken)
    {
        const string subDirectory = "Charity";

        try
        {
            _logger.LogInformation("Handling CreateCharityCommand. Starting file upload for charity logo.");

            // Upload file
            var fileServiceResponse = await _fileManagerService.UploadFileAsync(new UpdateFileRequestModel
            {
                SubDirectory = subDirectory, Extension = command.FileExtension, FileBytes = command.Base64File
            });

            _logger.LogInformation("File upload successful. File path: {FilePath}",
                Path.Combine(_uploadDirectory, subDirectory, fileServiceResponse.FileName));

            // Create charity
            _logger.LogInformation("Creating charity with Name: {CharityName}", command.Name);
            var charity = Charity.Create(
                command.Name,
                command.Description,
                command.Website,
                1, // CreatedByUserId
                command.Address,
                2, // CityId
                command.Telephone,
                command.ManagerName,
                1, // SocialId
                command.ContactName,
                command.ContactPhone
            );

            _logger.LogInformation("Charity created successfully. Charity Id: {CharityId}", charity.Id);

            // Set logo
            _logger.LogInformation("Setting logo for charity with FileName: {FileName}", fileServiceResponse.FileName);
            charity.SetLogo(
                fileServiceResponse.FileName,
                Path.Combine(_uploadDirectory, subDirectory, fileServiceResponse.FileName),
                "application/octet-stream"
            );

            // Insert charity into repository and commit
            _logger.LogInformation("Inserting charity into repository and committing changes.");
            await _charityCommandRepository.InsertAsync(charity);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Charity successfully created and committed. Charity Id: {CharityId}", charity.Id);

            return charity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating charity.");
            throw; // Re-throw the exception after logging
        }
    }
}