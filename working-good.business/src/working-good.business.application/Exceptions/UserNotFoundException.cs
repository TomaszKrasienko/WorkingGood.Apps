using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class UserNotFoundException(string message, string messageCode) 
    : AuthorizeCustomException(message, messageCode);