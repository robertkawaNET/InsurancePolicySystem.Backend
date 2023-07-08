using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost, Route("auth")]
    public async Task<IActionResult> Authenticate([FromBody] UserToAuthenticateDto request)
    {
        var user = await _userRepository.GetByLoginAsync(new Login(request.Login));
        if (user is null)
        {
            return NotFound();
        }

        if (!request.Password.Equals(user.Password.Value))
        {
            return BadRequest("Password is incorrect");
        }

        return Ok("Login success");
    }
}

public class UserToAuthenticateDto
{
    public string Login { get; set; }
    public string Password { get; set; }
}