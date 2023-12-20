using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class UserCanNotBeLoggedException(Guid userId)
    : AuthorizeCustomException($"User {userId} can not be logged", "user_can_be_logged");