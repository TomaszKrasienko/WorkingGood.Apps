using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Models.Company;

namespace working_good.business.core.DomainServices;

internal sealed class CompanyRegistrationService : ICompanyRegistrationService
{
    public Company RegisterCompany(List<Company> companies, Guid id, string name, bool isOwner, string emailDomain,
        TimeSpan? slaTimeSpan = null)
    {
        var ownerCompany = companies?.FirstOrDefault(x => x.IsOwner) ?? null;
        switch (isOwner)
        {
            case true when ownerCompany is not null:
                throw new OwnerCompanyAlreadyExistsException();
            case false when !(IsOwnerCompanyFullyRegistered(ownerCompany)):
                throw new OwnerCompanyDoesNotExistsException();
        }

        var isNameUnique = companies?.Any(x => x.Name == name) ?? false;
        if (isNameUnique)
        {
            throw new CompanyNameAlreadyExistsException(name);
        }

        var isDomainEmailUnique = companies?.Any(x => x.EmailDomain == emailDomain) ?? false;
        if (isDomainEmailUnique)
        {
            throw new CompanyEmailDomainAlreadyExists(emailDomain);
        }
        
        return isOwner 
            ? Company.CreateOwnerCompany(id, name, emailDomain) 
            : Company.CreateCompany(id, name, slaTimeSpan, emailDomain);
    }

    private bool IsOwnerCompanyFullyRegistered(Company ownerCompany)
        => ownerCompany is not null && ownerCompany.Employees.Any(x => x.User != null);
}