namespace BlogSystem.Application.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException(string message) : base(message, 403)
    {
    }
}
