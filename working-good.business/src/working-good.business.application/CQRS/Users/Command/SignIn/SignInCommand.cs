using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Users.Command.SignIn;

public record SignInCommand(string Email, string Password) : ICommand;