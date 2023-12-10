namespace working_good.business.core.Exceptions;

public sealed class InvalidFullNameException(string firstName, string lastName)
    : CustomException($"Values {firstName}, {lastName} are invalid", "invalid_full_name");