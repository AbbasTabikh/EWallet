using EWallet.Entities;
using EWallet.Models;
using EWallet.Services.Interfaces;
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
            //get the configurations
            double days = _configuration.GetSection("Jwt:AccessTokenExpiration").Get<double>();
            string issuer = _configuration.GetSection("Jwt:Issuer").Get<string>()!;
            string audience = _configuration.GetSection("Jwt:Audience").Get<string>()!;
            string secret = _configuration.GetSection("Jwt:Secret").Get<string>()!;

            var accessTokenExpiration = DateTime.UtcNow.AddDays(days);
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var securityToken = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
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
