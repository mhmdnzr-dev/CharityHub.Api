namespace CharityHub.Core.Contract.Features.Charity.Queries.GetCharityById;

public sealed class CharityByIdResponseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<SocialDtoModel>? Socials { get; set; }
    public string BannerUriAddress { get; set; }
    public string LogoUriAddress { get; set; }
    public int? CityId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public string Telephone { get; set; }
    public string ManagerName { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
}

public sealed class SocialDtoModel
{
    public string? Name { get; set; }
    public string? Url { get; set; }
}