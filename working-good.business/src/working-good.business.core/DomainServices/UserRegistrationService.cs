using working_good.business.core.Abstractions;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Exceptions;
using working_good.business.core.Models.Company;
using working_good.business.core.Policies.Abstractions;
using working_good.business.core.ValueObjects.User;

namespace working_good.business.core.DomainServices;

internal sealed class UserRegistrationService(IPasswordManager passwordManager, IPasswordPolicy passwordPolicy)
    : IUserRegistrationService
{
    public Company RegisterNewUser(List<Company> companies, Guid companyId, Guid id, string email, string firstName, string lastName,
        string password, string role)
    {
        var isCompanyExists = companies.Any(x => x.Id == companyId);
        if (!isCompanyExists)
        {
            throw new CompanyDoesNotExistException(companyId);
        }
        var isCompanyWithUserExists = companies.Any(x => x.Users.Any(arg => arg.Email == email));
        if (isCompanyWithUserExists)
        {
            throw new EmailAlreadyExistException(email);
        }

        var company = companies.Single(x => x.Id == companyId);
        company.RegisterUser(passwordPolicy, passwordManager, id, email, new FullName(firstName, lastName), password, role);
        return company;
    }
}