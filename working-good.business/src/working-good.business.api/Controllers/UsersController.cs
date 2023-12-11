using Microsoft.AspNetCore.Mvc;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.application.CQRS.Users.Command.SignUp;

namespace working_good.business.api.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ICommandHandler<SignUpCommand> _signUpCommandHandler;
    
    public UsersController(ICommandHandler<SignUpCommand> signUpCommandHandler)
    {
        _signUpCommandHandler = signUpCommandHandler;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpCommand command)
    {
        return Ok();
    }
}