namespace CharityHub.Core.Application.Exceptions;
internal sealed class ApplicationException : Exception
{
    protected ApplicationException(string message) : base(message) { }

    public static ApplicationException ValidationFailed(string details) =>
       new ApplicationException($"Validation failed: {details}");

    public static ApplicationException OperationFailed(string operation) =>
        new ApplicationException($"Operation failed: {operation}");
}