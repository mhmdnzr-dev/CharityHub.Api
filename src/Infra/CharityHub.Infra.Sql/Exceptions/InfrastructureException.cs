namespace CharityHub.Infra.Sql.Exceptions;


internal sealed class InfrastructureException : Exception
{
    protected InfrastructureException(string message) : base(message) { }

    public static InfrastructureException DatabaseError(string details) =>
        new InfrastructureException($"Database error: {details}");

    public static InfrastructureException NetworkError(string details) =>
        new InfrastructureException($"Network error: {details}");
}