using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Users.Command.VerifyAccount;

public record VerifyAccountCommand(string VerificationToken) : ICommand;