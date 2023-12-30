namespace working_good.business.core.Exceptions;

public sealed class NotMatchingEmployeeEmailDomainException(string emailDomain)
    : CustomException($"Employee email domain does not match with company email domain",
        "not_matching_employee_email_domain");
