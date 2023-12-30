namespace working_good.business.core.Exceptions;

public sealed class InvalidTitleException() 
    : CustomException("Title can not be empty", "invalid_title");