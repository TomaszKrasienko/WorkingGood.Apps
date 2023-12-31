using working_good.business.core.Models.Company;

namespace working_good.business.core.DomainServices.Abstractions;

public interface IUserRegistrationService
{
    Company RegisterNewUser(List<Company> companies, Guid employeeId, Guid id, string firstName,
        string lastName, string password);
}