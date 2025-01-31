namespace CharityHub.Infra.Identity.Models.Token.Requests;

using Core.Domain.Entities.Identity;

public class GenerateTokenRequest
{
    public ApplicationUser User { get; set; }
}