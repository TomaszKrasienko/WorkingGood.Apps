using working_good.business.core.Exceptions;

namespace working_good.business.core.Models.Company;

public sealed class InvalidEmailDomainException()
    : CustomException("Email domain can not be empty", "invalid_email_domain");