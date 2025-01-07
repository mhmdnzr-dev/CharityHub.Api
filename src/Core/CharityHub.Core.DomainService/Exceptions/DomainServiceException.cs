namespace CharityHub.Core.DomainService.Exceptions;

internal sealed class DomainServiceException : Exception
{
    protected DomainServiceException(string message) : base(message) { }

    public static DomainServiceException ValidationFailed(string details) =>
       new DomainServiceException($"Validation failed: {details}");

    public static DomainServiceException OperationFailed(string operation) =>
        new DomainServiceException($"Operation failed: {operation}");
}