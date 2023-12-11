using working_good.business.core.Exceptions;

namespace working_good.business.application.CQRS.Users.Command.SignUp;

public sealed class EmailAlreadyExistException(string email)
    : CustomException($"Email: {email} already in use", "email_already_exists");