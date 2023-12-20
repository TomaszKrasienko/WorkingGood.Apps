using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class UserNotActiveException(string message, string messageCode)
    : AuthorizeCustomException(message, messageCode);