using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Jose;

namespace AccessControlAPI.Models
{
    public class JwtTokenGen
    {
        public (string token, double expiresIn) GenerateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("f829c1b6f4b49ac0fef262342b2d8d88");
            var tokenExpiration = DateTime.UtcNow.AddHours(2);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Email, email)
                }),
                Expires = tokenExpiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var timeToExpire = (tokenExpiration - DateTime.UtcNow).TotalSeconds;

            var encryptedToken = EncryptToken(tokenString, key);

            return (encryptedToken, timeToExpire);
        }

        private static string EncryptToken(string token, byte[] key)
        {
            return Jose.JWT.Encode(token, key, JweAlgorithm.A256GCMKW, JweEncryption.A256GCM);
        }
    }
}
