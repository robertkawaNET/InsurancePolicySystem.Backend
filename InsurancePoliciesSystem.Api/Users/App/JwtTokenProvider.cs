using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InsurancePoliciesSystem.Api.Users.Domain;
using Microsoft.IdentityModel.Tokens;

namespace InsurancePoliciesSystem.Api.Users.App;

public class JwtTokenProvider
{
    private readonly IConfiguration _configuration;

    public JwtTokenProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateJwt(User user)
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
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = credentials
        };
        
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }
}