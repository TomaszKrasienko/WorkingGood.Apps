using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Models.Company;

namespace working_good.business.core.DomainServices;

internal sealed class CompanyRegistrationService : ICompanyRegistrationService
{
    public Company RegisterCompany(List<Company> companies, Guid id, string name, bool isOwner, string emailDomain,
        TimeSpan? slaTimeSpan = null)
    {
        var isOwnerCompanyExists = companies?.Any(arg => arg.IsOwner) ?? false;
        switch (isOwnerCompanyExists)
        {
            case true when isOwner:
                throw new OwnerCompanyAlreadyExistsException();
            case false when !isOwner:
                throw new OwnerCompanyDoesNotExistsException();
        }

        var isNameUnique = companies?.Any(x => x.Name == name) ?? false;
        if (isNameUnique)
        {
            throw new CompanyNameAlreadyExistsException(name);
        }

        return isOwner 
            ? Company.CreateOwnerCompany(id, name, emailDomain) 
            : Company.CreateCompany(id, name, slaTimeSpan, emailDomain);
    }
}