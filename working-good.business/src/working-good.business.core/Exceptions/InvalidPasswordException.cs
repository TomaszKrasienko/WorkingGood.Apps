namespace working_good.business.core.Exceptions;

public sealed class InvalidPasswordException(string value)
    : CustomException($"Provided value {value} is invalid for password");