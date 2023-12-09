using EWallet.Entities;
using EWallet.Models;
using EWallet.Services.Interfaces;
using EWallet.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EWallet.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AccessToken GenerateToken(User user)
        {
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtConfiguration>()!;
            var accessTokenExpiration = DateTime.UtcNow.AddDays(jwtOptions.AccessTokenExpiration);
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var securityToken = new JwtSecurityToken
            (
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: userClaims,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
            );

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityHandler.WriteToken(securityToken);
            return new AccessToken
            {
                Token = token,
                Expiration = ((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds()
            };
        }
    }
}
