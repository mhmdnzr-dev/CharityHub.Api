namespace CharityHub.Core.Application.Services.Terms.Queries.GetLastTerm;
using System.Threading;
using System.Threading.Tasks;

using Contract.Primitives.Handlers;
using Contract.Terms.Queries;
using Contract.Terms.Queries.GetLastTerm;

public class GetLastTermQueryHandler : IQueryHandler<GetLastTermQuery, LastTermResponseDto>
{
    private readonly ITermQueryRepository _termQueryRepository;

    public GetLastTermQueryHandler(ITermQueryRepository termQueryRepository)
    {
        _termQueryRepository = termQueryRepository;
    }

    public async Task<LastTermResponseDto> Handle(GetLastTermQuery query, CancellationToken cancellationToken)
    {
        // Logic to handle the query, for example:
        return new LastTermResponseDto
        {
            Content = "Test"
        };
    }
}
