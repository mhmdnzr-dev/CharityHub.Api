namespace CharityHub.Core.Contract.Charity.Queries.GetCharityById;

public sealed class CharityByIdResponseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<SocialDtoModel>? Socials { get; set; }
    public string PhotoUriAddress { get; set; }
    public string BannerUriAddress { get; set; }
}

public sealed class SocialDtoModel
{
    public string? Name { get; set; }
    public string? Url { get; set; }
}