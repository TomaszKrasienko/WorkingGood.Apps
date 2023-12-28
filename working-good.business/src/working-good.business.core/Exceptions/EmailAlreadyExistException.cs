namespace working_good.business.core.Exceptions;

public sealed class EmailAlreadyExistException(string email)
    : CustomException($"Email: {email} already in use", "email_already_exists");