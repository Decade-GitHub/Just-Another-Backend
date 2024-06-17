using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Jose;
using System.Security.Claims;

namespace AccessControlAPI.Models
{
    public class JwtTokenValidator(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public ClaimsPrincipal ValidateToken(string encryptedToken)
        {
            var secretString = (_configuration["SecretKey"]?.ToString()) ?? throw new ArgumentException("This is impossible.");
            var key = Encoding.ASCII.GetBytes(secretString);
            var decryptedToken = Jose.JWT.Decode(encryptedToken, key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            var principal = tokenHandler.ValidateToken(decryptedToken, validationParameters, out _);

            return principal;
        }
    }
}
