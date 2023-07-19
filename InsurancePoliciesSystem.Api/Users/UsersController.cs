using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InsurancePoliciesSystem.Api.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UsersController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
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

        var token = CreateJwt(user);
        
        return Ok(new {token});
    }
    
    private string CreateJwt(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);
        var identity = new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, $"{user.Role.ToString()}"),
            new(ClaimTypes.Name,$"{user.Login.Value}")
        });

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials
        };
        
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }

}

public class UserToAuthenticateDto
{
    public string Login { get; set; }
    public string Password { get; set; }
}