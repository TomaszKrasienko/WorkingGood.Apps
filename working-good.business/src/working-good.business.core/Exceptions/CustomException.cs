namespace working_good.business.core.Exceptions;

public abstract class CustomException : Exception
{
    public string MessageCode { get; }
    
    protected CustomException(string messageCode) : base()
    {
        MessageCode = messageCode;
    }

    protected CustomException(string message, string messageCode) : base(message)
    {
        MessageCode = messageCode;
    }
}