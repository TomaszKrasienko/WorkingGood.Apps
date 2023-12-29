using working_good.business.core.Models.Company;

namespace working_good.business.core.DomainServices.Abstractions;

public interface ICompanyRegistrationService
{
    Company RegisterCompany(List<Company> companies, Guid id, string name, bool isOwner, 
        string emailDomain, TimeSpan? slaTimeSpan = null);
}