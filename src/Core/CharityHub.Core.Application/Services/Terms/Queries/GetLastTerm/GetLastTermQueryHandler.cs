namespace CharityHub.Core.Application.Services.Terms.Queries.GetLastTerm;

using System.Threading;
using System.Threading.Tasks;

using Contract.Primitives.Handlers;
using Contract.Terms.Queries;
using Contract.Terms.Queries.GetLastTerm;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetLastTermQueryHandler : QueryHandlerBase<GetLastTermQuery, LastTermResponseDto>
{
    private readonly ITermQueryRepository _termQueryRepository;


    public GetLastTermQueryHandler(IMemoryCache cache, ITermQueryRepository termQueryRepository) : base(cache)
    {
        _termQueryRepository = termQueryRepository;
    }

    public override async Task<LastTermResponseDto> Handle(GetLastTermQuery query, CancellationToken cancellationToken)
    {
        var result = await _termQueryRepository.GetAllAsync();
        return new LastTermResponseDto { Content = "Test" };
    }
}