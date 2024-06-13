using Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;


public class TokenGenerator : ITokenGenerator
{
    private readonly UserManager<User> _userManager;
    private readonly SymmetricSecurityKey _symmetricSecurityKey;
    private readonly IConfiguration _configuration;
    public TokenGenerator(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _configuration = config;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["ApiSettings:JwtOptions:Secret"]));

    }

    public async Task<string> GenerateToken(User user)
    {

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)

            };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["ApiSettings:JwtOptions:Audience"],
            Issuer = _configuration["ApiSettings:JwtOptions:Issuer"],
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
