using working_good.business.core.Exceptions;

namespace working_good.business.core.Models.Company;

public sealed class InvalidUserEmailDomainException : CustomException
{
    public InvalidUserEmailDomainException(string emailDomain) 
        : base($"User email domain does not match with company email domain",
            "invalid_user_email_domain")
    {
    }
}