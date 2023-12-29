using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.CQRS.Companies.Commands.Register;

internal sealed class RegisterCompanyCommandHandler : ICommandHandler<RegisterCompanyCommand>
{
    public Task HandleAsync(RegisterCompanyCommand command, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}