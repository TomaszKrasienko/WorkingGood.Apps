using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Employees.Commands;

public record AddEmployeeCommand(Guid CompanyId, Guid Id, string Email) : ICommand;