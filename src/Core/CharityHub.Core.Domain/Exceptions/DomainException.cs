namespace CharityHub.Core.Domain.Exceptions;
internal sealed class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }

    public static DomainException NotFound(string entity) =>
        new DomainException($"{entity} not found.");
}