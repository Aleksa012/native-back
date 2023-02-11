using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using NativeBackendApp.Models;

namespace NativeBackend.Services;

public class Auth
{
    private readonly IConfiguration _configuration;

    public Auth(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetToken(User user)
    {
        var issuer = Environment.GetEnvironmentVariable("ISSUER") ?? _configuration["Jwt:Issuer"];
        var audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
#pragma warning disable CS8604
            (Environment.GetEnvironmentVariable("SECRET_TOKEN_KEY") ?? _configuration["Jwt:Key"]);
#pragma warning restore CS8604
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            }),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        
        return stringToken;
    }
    
}