using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Companies.Commands.Register;
using working_good.business.application.CQRS.Companies.Queries;
using working_good.business.application.CQRS.Companies.Queries.GetCompanies;
using working_good.business.application.CQRS.Companies.Queries.GetCompanyById;
using working_good.business.application.CQRS.Employees.Commands;
using working_good.business.application.CQRS.Employees.Queries.GetEmployees;
using working_good.business.application.CQRS.Users.Command.SignIn;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.application.CQRS.Users.Command.VerifyAccount;
using working_good.business.application.CQRS.Users.Queries.GetAvailableResources;
using working_good.business.application.DTOs;
using working_good.business.application.Services;
using working_good.business.application.Services.Security;

namespace working_good.business.api.Controllers;

[Route("companies")]
[ApiController]
public sealed class CompaniesController(
    IQueryHandler<GetCompanyByIdQuery, CompanyDto> getCompanyQueryHandler,
    IQueryHandler<GetCompaniesQuery, QueryPaginationDto<IEnumerable<CompanyDto>>> getCompaniesQueryHandler,
    IQueryHandler<GetAvailableUserRolesQuery, List<string>> userRolesHandler,
    IQueryHandler<GetEmployeesQuery, QueryPaginationDto<IEnumerable<EmployeeDto>>> getEmployeesQueryHandler,
    ICommandHandler<RegisterCompanyCommand> registerCompanyCommandHandler,
    ICommandHandler<AddEmployeeCommand> addEmployeeCommandHandler,
    ICommandHandler<SignUpCommand> signUpCommandHandler,
    ICommandHandler<VerifyAccountCommand> verifyCommandHandler,
    ICommandHandler<SignInCommand> signInCommandHandler,
    IAccessTokenStorage accessTokenStorage
    ) : ControllerBase
{
    [HttpGet("{companyId:guid}")]
    public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await getCompanyQueryHandler.HandleAsync(new GetCompanyByIdQuery(companyId), cancellationToken);
        return Ok(company);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] GetCompaniesQuery getCompaniesQuery, CancellationToken cancellationToken)
    {
        var companies =
            await getCompaniesQueryHandler.HandleAsync(getCompaniesQuery, cancellationToken);
        return Ok(companies);
    }

    [HttpGet("{companyId:guid}/employees")]
    public async Task<IActionResult> GetEmployeeByCompanyId([FromRoute] Guid companyId, [FromQuery] PaginationArgumentsDto paginationArgumentsDto, CancellationToken cancellationToken)
    {
        return Ok(await getEmployeesQueryHandler.HandleAsync(new GetEmployeesQuery(companyId)
        {
            PageNumber = paginationArgumentsDto.PageNumber, 
            PageSize = paginationArgumentsDto.PageSize
        }, cancellationToken));
    }
    
    [HttpGet("employees/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeById(Guid employeeId)
    {
        return Ok();
    }
    
    [HttpGet("employees/users/get-available-roles")]
    public async Task<ActionResult<List<string>>> GetAvailableRoles(CancellationToken cancellationToken)
    {
        var result = await userRolesHandler.HandleAsync(new GetAvailableUserRolesQuery(), cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("register-company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyCommand command, CancellationToken cancellationToken)
    {
        var companyId = Guid.NewGuid();
        await registerCompanyCommandHandler.HandleAsync(command with {Id = companyId}, cancellationToken);
        return CreatedAtAction(nameof(GetCompanyById), new {companyId}, null);
    }

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