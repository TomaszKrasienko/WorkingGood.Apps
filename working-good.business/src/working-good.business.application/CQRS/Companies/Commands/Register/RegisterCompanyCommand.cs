namespace working_good.business.application.CQRS.Companies.Commands.Register;

public record RegisterCompanyCommand(Guid Id, string Name, bool IsOwner, TimeSpan SlaTimeSpan,
    string EmailDomain);