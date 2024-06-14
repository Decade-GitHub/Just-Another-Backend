using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Jose;
using System.Security.Claims;

namespace AccessControlAPI.Models
{
    public class JwtTokenValidator
    {
        public ClaimsPrincipal ValidateToken(string encryptedToken)
        {
            var key = Encoding.ASCII.GetBytes("f829c1b6f4b49ac0fef262342b2d8d88");
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
