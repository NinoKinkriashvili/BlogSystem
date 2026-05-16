namespace BlogSystem.Application.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; set; }

    protected BaseException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
