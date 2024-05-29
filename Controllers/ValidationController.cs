using AcessControlAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AcessControlAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly JwtTokenValidator _jwtTokenValidator;

        public ValidationController()
        {
            var key = "f829c1b6f4b49ac0fef262342b2d8d88";
            _jwtTokenValidator = new JwtTokenValidator(key);
        }
        [HttpGet("validate")]
        public IActionResult ValidateToken([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Token is required.");
            }

            var principal = _jwtTokenValidator.ValidateToken(token);

            if (principal == null)
            {
                return Unauthorized("Invalid token.");
            }

            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return Unauthorized("Email claim is missing in the token");
            }

            var expClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim == null || !long.TryParse(expClaim.Value, out long exp))
            {
                return Unauthorized("Expiration claim is missing or invalid in the token");
            }

            var expiresIn = (int)(DateTimeOffset.FromUnixTimeSeconds(exp) - DateTimeOffset.UtcNow).TotalSeconds;

            return Ok(new
            {
                email = emailClaim.Value,
                expires_in = expiresIn
            });
        }
    }
}
