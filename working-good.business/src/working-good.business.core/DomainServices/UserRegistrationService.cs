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
    public Company RegisterNewUser(List<Company> companies, Guid employeeId, Guid id, string firstName, 
        string lastName, string password, string role)
    {
        var company = companies.FirstOrDefault(x => x.Employees.Any(arg => arg.Id == employeeId));
        if (company is null)
        {
            throw new CompanyForEmployeeDoesNotExistException(employeeId);
        }

        var employee = company.Employees.First(x => x.Id == employeeId);
        if (employee.User is not null)
        {
            throw new UserAlreadyExistsException(employeeId);
        }
        
        company.RegisterUser(passwordPolicy, passwordManager, employeeId, id, new FullName(firstName, lastName),
            password, role);
        return company;
    }
}