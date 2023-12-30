namespace working_good.business.core.Exceptions;

public sealed class InvalidEmailDomainException()
    : CustomException("Email domain can not be empty", "invalid_email_domain");