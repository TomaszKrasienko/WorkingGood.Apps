using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Users.Command.SignUp;
using working_good.business.application.CQRS.Users.Queries.GetAvailableResources;

namespace working_good.business.api.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ICommandHandler<SignUpCommand> _signUpCommandHandler;
    private readonly IQueryHandler<GetAvailableUserRolesQuery, List<string>> _userRolesHandler;
    public UsersController(ICommandHandler<SignUpCommand> signUpCommandHandler, 
        IQueryHandler<GetAvailableUserRolesQuery, List<string>> userRolesHandler)
    {
        _signUpCommandHandler = signUpCommandHandler;
        _userRolesHandler = userRolesHandler;
    }
    
    [HttpGet("get-available-roles")]
    public async Task<ActionResult<List<string>>> GetAvailableRoles(CancellationToken cancellationToken)
    {
        var result = await _userRolesHandler.HandleAsync(new GetAvailableUserRolesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpCommand command)
    {
        return Ok();
    }
}