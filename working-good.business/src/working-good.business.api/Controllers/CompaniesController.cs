using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Companies.Commands.Register;
using working_good.business.application.CQRS.Companies.Queries;
using working_good.business.application.DTOs;
using working_good.business.core.Models.Company;

namespace working_good.business.api.Controllers;

[Route("companies")]
[ApiController]
public sealed class CompaniesController(
    IQueryHandler<GetCompanyQuery, CompanyDto> getCompanyQueryHandler,
    ICommandHandler<RegisterCompanyCommand> registerCompanyCommandHandler
    ) : ControllerBase
{
    [HttpGet("{companyId:guid}")]
    public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await getCompanyQueryHandler.HandleAsync(new GetCompanyQuery(companyId), cancellationToken);
        return Ok(company);
    }
    
    [HttpPost("register-company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        var companyId = Guid.NewGuid();
        await registerCompanyCommandHandler.HandleAsync(command with {Id = companyId}, cancellationToken);
        return CreatedAtAction(nameof(GetCompanyById), new {companyId}, null);
    }
}