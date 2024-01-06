using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Companies.Commands.Register;
using working_good.business.application.CQRS.Companies.Commands.RegisterOwnerCompany;
using working_good.business.application.CQRS.Companies.Queries.GetCompanies;
using working_good.business.application.CQRS.Companies.Queries.GetCompanyById;
using working_good.business.application.CQRS.Companies.Queries.IsOwnerCompanyRegistered;
using working_good.business.application.CQRS.Employees.Commands;
using working_good.business.application.CQRS.Employees.Queries.GetEmployees;
using working_good.business.application.CQRS.Users.Command.SignIn;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.application.CQRS.Users.Command.VerifyAccount;
using working_good.business.application.DTOs;
using working_good.business.application.Services.Security;

namespace working_good.business.api.Controllers;

[Route("companies")]
[ApiController]
public sealed class CompaniesController(
    IQueryHandler<GetCompanyByIdQuery, CompanyDto> getCompanyQueryHandler,
    IQueryHandler<GetCompaniesQuery, QueryPaginationDto<IEnumerable<CompanyDto>>> getCompaniesQueryHandler,
    IQueryHandler<GetEmployeesQuery, QueryPaginationDto<IEnumerable<EmployeeDto>>> getEmployeesQueryHandler,
    IQueryHandler<IsOwnerCompanyRegisteredQuery, bool> isOwnerCompanyRegisteredQueryHandler,
    ICommandHandler<RegisterCompanyCommand> registerCompanyCommandHandler,
    ICommandHandler<RegisterOwnerCompanyCommand> registerOwnerCommandHandler,
    ICommandHandler<AddEmployeeCommand> addEmployeeCommandHandler,
    ICommandHandler<SignUpCommand> signUpCommandHandler,
    ICommandHandler<VerifyAccountCommand> verifyCommandHandler,
    ICommandHandler<SignInCommand> signInCommandHandler,
    IAccessTokenStorage accessTokenStorage
    ) : ControllerBase
{
    [HttpGet("is-owner-registered")]
    public async Task<ActionResult<bool>> IsOwnerCompanyRegistered(CancellationToken cancellationToken)
        => Ok(await isOwnerCompanyRegisteredQueryHandler.HandleAsync(new IsOwnerCompanyRegisteredQuery(),
            cancellationToken));
    
    //Todo: Tu by się przydało Policy
    [Authorize]
    [HttpGet("{companyId:guid}")]
    public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await getCompanyQueryHandler.HandleAsync(new GetCompanyByIdQuery(companyId), cancellationToken);
        return Ok(company);
    }

    [HttpGet]
    [Authorize(Roles = "Manager, Employee")]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] GetCompaniesQuery getCompaniesQuery, CancellationToken cancellationToken)
    {
        var companies =
            await getCompaniesQueryHandler.HandleAsync(getCompaniesQuery, cancellationToken);
        return Ok(companies);
    }

    //Todo: Tu by się przydało Policy
    [Authorize]
    [HttpGet("{companyId:guid}/employees")]
    public async Task<IActionResult> GetEmployeeByCompanyId([FromRoute] Guid companyId, [FromQuery]int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        return Ok(await getEmployeesQueryHandler.HandleAsync(new GetEmployeesQuery(companyId)
        {
            PageNumber = pageNumber, 
            PageSize = pageSize
        }, cancellationToken));
    }
    
    //Todo: Tu by się przydało Policy
    [Authorize]
    [HttpGet("employees/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid employeeId)
    {
        return Ok();
    }

    [HttpPost("register-owner-company")]
    public async Task<IActionResult> RegisterOwenCompany(RegisterOwnerCompanyCommand command,
        CancellationToken cancellationToken)
    {
        await registerOwnerCommandHandler.HandleAsync(command with
        {
            CompanyId = Guid.NewGuid(), 
            EmployeeId = Guid.NewGuid(), 
            UserId = Guid.NewGuid()
        }, cancellationToken);
        return Created();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("register-company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        var companyId = Guid.NewGuid();
        await registerCompanyCommandHandler.HandleAsync(command with {Id = companyId}, cancellationToken);
        return CreatedAtAction(nameof(GetCompanyById), new {companyId}, null);
    }

    [Authorize(Roles = "Manager, Employee")]
    [HttpPost("{companyId}/add-employee")]
    public async Task<IActionResult> AddEmployee(Guid companyId, AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeId = Guid.NewGuid();
        await addEmployeeCommandHandler.HandleAsync(command with { Id = employeeId, CompanyId = companyId}, cancellationToken);
        return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = employeeId }, null);
    }
    
    [HttpPost("employees/{employeeId:guid}/sign-up")]
    public async Task<IActionResult> SignUp(Guid employeeId, SignUpCommand command, CancellationToken cancellationToken)
    {
        await signUpCommandHandler.HandleAsync(command with
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId
        }, cancellationToken);
        return Created();
    }
    
    [HttpPost("employees/users/verify-account")]
    public async Task<IActionResult> VerifyAccount(VerifyAccountCommand command, CancellationToken cancellationToken)
    {
        await verifyCommandHandler.HandleAsync(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("employees/users/sign-in")]
    public async Task<ActionResult<AccessTokenDto>> SignIn(SignInCommand command, CancellationToken cancellationToken)
    {
        await signInCommandHandler.HandleAsync(command, cancellationToken);
        var accessToken = accessTokenStorage.Get();
        return Ok(accessToken);
    }

}