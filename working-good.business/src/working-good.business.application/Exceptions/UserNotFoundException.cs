using working_good.business.core.Exceptions;

namespace working_good.business.application.Exceptions;

public sealed class UserNotFoundException(string email) 
    : AuthorizeCustomException($"Not found user with email: {email}", "user_not_found");