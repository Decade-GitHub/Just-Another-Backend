using Microsoft.AspNetCore.Mvc;
using AcessControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AcessControlAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenGen _jwtTokenGenerator;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
            _jwtTokenGenerator = new JwtTokenGen();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUser([FromBody] UserRequestModel validationRequest)
        {
            if (validationRequest == null || string.IsNullOrWhiteSpace(validationRequest.Email) || string.IsNullOrWhiteSpace(validationRequest.Password))
            {
                return BadRequest();
            }
            if (validationRequest.Password.Length < 8 || validationRequest.Password.Length > 32)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool emailExists = await EmailExistsAsync(validationRequest.Email);

                if (!emailExists)
                {
                    return new BadRequestObjectResult(ErrorResult.ErrorCodeReturner("Email não existe."));
                }
            }
            var (token, expiresIn) = _jwtTokenGenerator.GenerateToken(validationRequest.Email);

            return Ok(new
            {
                token,
                expires_in = expiresIn
            });
        }
        private async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
