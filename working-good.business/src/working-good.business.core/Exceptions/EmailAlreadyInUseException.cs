namespace working_good.business.core.Exceptions;

public sealed class EmailAlreadyInUseException(string email)
    : CustomException($"Email: {email} already in use", "email_already_in_use");