namespace CharityHub.Core.Contract.Features.Charity.Queries.GetCharityById;

using GetAllCharities;

using Primitives.Handlers;

public class GetCharityByIdQuery : IQuery<CharityByIdResponseDto>
{
    public int Id { get; set; }
}