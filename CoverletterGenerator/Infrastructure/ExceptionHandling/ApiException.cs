namespace CoverletterGenerator.Infrastructure.ExceptionHandling
{
    public abstract class ApiException(string message, int statusCode) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}
