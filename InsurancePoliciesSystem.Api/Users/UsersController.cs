using InsurancePoliciesSystem.Api.Users.App;
using InsurancePoliciesSystem.Api.Users.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenProvider _jwtTokenProvider;

    public UsersController(IUserRepository userRepository, JwtTokenProvider jwtTokenProvider)
    {
        _userRepository = userRepository;
        _jwtTokenProvider = jwtTokenProvider;
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

        var token = _jwtTokenProvider.CreateJwt(user);
        
        return Ok(new {token});
    }
}