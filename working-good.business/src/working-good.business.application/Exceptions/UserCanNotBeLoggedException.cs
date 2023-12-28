using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class UserCanNotBeLoggedException(string email)
    : AuthorizeCustomException($"User with email: {email} can not be logged", "user_can_be_logged");