using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.application.CQRS.Users.Queries.GetAvailableResources;

namespace working_good.business.api.Controllers;

[Route("users")]
[ApiController]
public sealed class UsersController(
    ICommandHandler<SignUpCommand> signUpCommandHandler,
    IQueryHandler<GetAvailableUserRolesQuery, List<string>> userRolesHandler)
    : ControllerBase
{
    [HttpGet("get-available-roles")]
    public async Task<ActionResult<List<string>>> GetAvailableRoles(CancellationToken cancellationToken)
    {
        var result = await userRolesHandler.HandleAsync(new GetAvailableUserRolesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken)
    {
        await signUpCommandHandler.HandleAsync(command with { Id = Guid.NewGuid() }, cancellationToken);
        return Created();
    }
    
}