using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Users.Command.SignIn;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.application.CQRS.Users.Queries.GetAvailableResources;
using working_good.business.application.DTOs;
using working_good.business.application.Services;

namespace working_good.business.api.Controllers;

[Route("users")]
[ApiController]
public sealed class UsersController(
    ICommandHandler<SignUpCommand> signUpCommandHandler,
    ICommandHandler<SignInCommand> signInCommandHandler,
    IQueryHandler<GetAvailableUserRolesQuery, List<string>> userRolesHandler,
    IAccessTokenStorage accessTokenStorage)
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

    [HttpPost("sign-in")]
    public async Task<ActionResult<AccessTokenDto>> SignIn(SignInCommand command, CancellationToken cancellationToken)
    {
        await signInCommandHandler.HandleAsync(command, cancellationToken);
        return Ok(accessTokenStorage.Get());
    }
}