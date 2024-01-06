
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Employees.Commands;
using working_good.business.application.CQRS.Users.Command.SignUp;

namespace working_good.business.application.CQRS.Companies.Commands.RegisterOwnerCompany;

public record RegisterOwnerCompanyCommand(Guid CompanyId, string Name, string EmailDomain,
    Guid EmployeeId, string EmployeeEmail, Guid UserId, string FirstName, string LastName, 
    string Password) : ICommand;