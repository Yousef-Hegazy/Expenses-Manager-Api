using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models.Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Security;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }
    
    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(type: ClaimTypes.Name, value: user.UserName ?? ""),
            new Claim(type: ClaimTypes.NameIdentifier, value: user.Id),
            new Claim(type: ClaimTypes.Email, value: user.Email ?? "")
        };
        
        var keyBytes = Encoding.UTF8.GetBytes(_config["TokenKey"]);
        var key = new SymmetricSecurityKey(keyBytes);

        var signingCredentials = new SigningCredentials(key: key, algorithm: SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddDays(7),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}