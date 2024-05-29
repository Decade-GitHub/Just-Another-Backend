using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Jose;
using System.Security.Claims;

namespace AcessControlAPI.Models
{
    public class JwtTokenValidator
    {
        private readonly byte[] _key;

        public JwtTokenValidator(string key)
        {
            _key = Encoding.ASCII.GetBytes(key);
        }
        public ClaimsPrincipal ValidateToken(string encryptedToken)
        {
            var decryptedToken = Jose.JWT.Decode(encryptedToken, _key);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key)
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(decryptedToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}
