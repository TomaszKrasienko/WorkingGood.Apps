using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Companies.Commands.Register;

namespace working_good.business.api.Controllers;

[Route("companies")]
[ApiController]
public sealed class CompaniesController(
    ICommandHandler<RegisterCompanyCommand> registerCompanyHandler
    ) : ControllerBase
{
    [HttpPost("register-company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        await registerCompanyHandler.HandleAsync(command, cancellationToken);
        return Created();
    }
}