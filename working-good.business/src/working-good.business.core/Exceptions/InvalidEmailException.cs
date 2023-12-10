namespace working_good.business.core.Exceptions;

public sealed class InvalidEmailException(string value)
    : CustomException($"Provided value {value} is invalid for email", "invalid_email");