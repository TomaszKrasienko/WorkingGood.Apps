using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Users.Command.SignUp;

public record SignUpCommand(Guid EmployeeId, Guid Id,
    string FirstName, string LastName, string Password, string Role) : ICommand;