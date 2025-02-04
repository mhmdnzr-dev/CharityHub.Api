namespace CharityHub.Core.Application.Primitives;

using CharityHub.Core.Contract.Primitives.Handlers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;
using Infra.Identity.Models.Token.Responses;

using Microsoft.AspNetCore.Http;


public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand<int>
{
    protected readonly ITokenService TokenService;
    private static IHttpContextAccessor _httpContextAccessor;

    protected CommandHandlerBase(ITokenService tokenService)
    {
        TokenService = tokenService;
    }

    public static void ConfigureHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public abstract Task<int> Handle(TCommand request, CancellationToken cancellationToken);

    protected string GetTokenFromHeader()
    {
        if (_httpContextAccessor?.HttpContext?.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) == true)
        {
            var token = authorizationHeader.ToString();

            if (!string.IsNullOrWhiteSpace(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
            }

            return token.Trim(); // Return the original token if "Bearer " is not found
        }

        return null; // Return null if the Authorization header is missing
    }

    protected async Task<GetUserByTokenResponse> GetUserDetailsAsync()
    {
        var token = GetTokenFromHeader();
        if (!string.IsNullOrEmpty(token))
        {
            return await TokenService.GetUserByTokenAsync(new GetUserByTokenRequest { Token = token });
        }
        return null;
    }
}
