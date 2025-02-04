namespace CharityHub.Core.Application.Primitives;

using Contract.Primitives.Handlers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models.Token.Requests;
using Infra.Identity.Models.Token.Responses;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

public abstract class QueryHandlerBase<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    protected readonly IMemoryCache Cache;
    protected readonly ITokenService TokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Inject IHttpContextAccessor directly into the constructor
    protected QueryHandlerBase(IMemoryCache cache, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        Cache = cache;
        TokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;  // Assign it here
    }

    public abstract Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);

    protected string GetTokenFromHeader()
    {
        // Use the injected _httpContextAccessor instead of the static one
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

