namespace working_good.business.core.Exceptions;

public abstract class AuthorizeCustomException : CustomException
{
    protected AuthorizeCustomException(string messageCode) : base(messageCode)
    {
    }

    protected AuthorizeCustomException(string message, string messageCode) : base(message, messageCode)
    {
    }
}