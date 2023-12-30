namespace working_good.business.core.Exceptions;

public class CompanyEmailDomainAlreadyExists(string emailDomain)
    : CustomException($"Company with email domain {emailDomain} already exists", "email_domain_already_exists");