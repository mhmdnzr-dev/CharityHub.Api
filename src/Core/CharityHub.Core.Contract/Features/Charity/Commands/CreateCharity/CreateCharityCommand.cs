namespace CharityHub.Core.Contract.Features.Charity.Commands.CreateCharity;

using Primitives.Handlers;

public class CreateCharityCommand : ICommand<int>
{
    public byte[] Base64File { get; set; }
    public string FileExtension { get; set; }

    /*public string SocialName { get; set; }
    public string SocialAbbreviation { get; set; }
    public string SocialUrl { get; set; }*/
    public string Name { get; set; }
    public string Description { get; set; }
    public string Website { get; set; }
    public string Address { get; set; }
    public int? CityId { get; set; }
    public string Telephone { get; set; }
    public string ManagerName { get; set; }

    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
}