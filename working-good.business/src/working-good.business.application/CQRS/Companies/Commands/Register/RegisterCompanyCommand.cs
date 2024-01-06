using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Companies.Commands.Register;

public record RegisterCompanyCommand(Guid Id, string Name, string EmailDomain,
    TimeSpan SlaTimeSpan) : ICommand;