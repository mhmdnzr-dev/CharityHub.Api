namespace CharityHub.Core.Contract.Charity.Queries.GetCharityById;

public class CharityByIdResponseDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<SocialModel> Socials { get; set; }
}

public sealed class SocialModel
{
    public string Name { get; set; }
    public string Url { get; set; }
}