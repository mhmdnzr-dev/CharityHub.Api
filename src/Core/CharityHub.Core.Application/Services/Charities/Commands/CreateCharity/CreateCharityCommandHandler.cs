namespace CharityHub.Core.Application.Services.Charities.Commands.CreateCharity;

using Contract.Charity.Commands.CreateCharity;
using Contract.Primitives.Handlers;

using Domain.Entities;

using CharityHub.Infra.FileManager.Interfaces;

using Contract.Charity.Commands;
using Contract.Configuration.Models;
using Contract.Primitives.Repositories;

using Infra.FileManager.Models.Requests;
using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Primitives;

public class CreateCharityCommandHandler : CommandHandlerBase<CreateCharityCommand>
{
    private readonly IFileManagerService _fileManagerService;
    private readonly ICharityCommandRepository _charityCommandRepository;
    private readonly ILogger<CreateCharityCommandHandler> _logger;
    private readonly string _uploadDirectory;


    public CreateCharityCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor,
        IFileManagerService fileManagerService, IUnitOfWork unitOfWork,
        ICharityCommandRepository charityCommandRepository, ILogger<CreateCharityCommandHandler> logger,
        IOptions<FileOptions> uploadDirectory) : base(tokenService, httpContextAccessor)
    {
        _fileManagerService = fileManagerService;
        _charityCommandRepository = charityCommandRepository;
        _logger = logger;
        _uploadDirectory = uploadDirectory.Value.UploadDirectory;
    }

    public override async Task<int> Handle(CreateCharityCommand command, CancellationToken cancellationToken)
    {
        const string subDirectory = "charities";

        var userDetails = await GetUserDetailsAsync();

        if (userDetails == null)
        {
            _logger.LogError("Failed to retrieve user details for CreatedByUserId: {CreatedByUserId}", userDetails?.Id);
            throw new InvalidOperationException("User details could not be found.");
        }

        try
        {
            _logger.LogInformation("Handling CreateCharityCommand. Starting file upload for charity logo.");

            // Ensure that the file is not null or empty
            if (command.Base64File == null || command.Base64File.Length == 0)
            {
                _logger.LogError("No file provided for charity logo upload.");
                throw new InvalidOperationException("No file provided for logo upload.");
            }

            // Upload file
            var fileServiceResponse = await _fileManagerService.UploadFileAsync(new UpdateFileRequestModel
            {
                SubDirectory = subDirectory, Extension = command.FileExtension, FileBytes = command.Base64File
            });

            _logger.LogInformation("File upload successful. File path: {FilePath}",
                Path.Combine(_uploadDirectory, subDirectory, fileServiceResponse.FileName));

            // Create charity
            _logger.LogInformation("Creating charity.");
            var charity = Charity.Create(
                command.Name,
                command.Description,
                command.Website,
                userDetails.Id,
                command.Address,
                command.CityId,
                command.Telephone,
                command.ManagerName,
                1,
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


            _logger.LogInformation("Charity successfully created and committed. Charity Id: {CharityId}", charity.Id);

            return charity.Id;
        }
        catch (IOException ioEx)
        {
            _logger.LogError(ioEx, "File upload failed due to I/O error.");
            throw new InvalidOperationException("File upload failed due to I/O error.", ioEx);
        }
        catch (UnauthorizedAccessException uaeEx)
        {
            _logger.LogError(uaeEx, "Access denied during file upload or charity creation.");
            throw new InvalidOperationException("Access denied during file upload or charity creation.", uaeEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating charity.");
            throw; // Re-throw the exception after logging
        }
    }
}