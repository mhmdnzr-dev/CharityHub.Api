namespace CharityHub.Core.Contract.Features.Charity.Commands.UpdateCharity;

using Primitives.Handlers;

public class UpdateCharityCommand : ICommand<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Website { get; set; }
    public int CityId { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public string Telephone { get; set; }
    public string ManagerName { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public byte[] BannerBase64 { get; set; }
    public byte[] LogoBase64 { get; set; }
    public string BannerFileExtension { get; set; }
    public string LogoFileExtension { get; set; }
}