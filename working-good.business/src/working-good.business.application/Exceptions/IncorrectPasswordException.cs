using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class IncorrectPasswordException(Guid userId)
    : AuthorizeCustomException($"For userId: {userId} password is incorrect", "incorrect_password");