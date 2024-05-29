using AcessControlAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AcessControlAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestModel request)
        {

            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new BadRequestObjectResult(ErrorResult.ErrorCodeReturner("Email e senha são necessários."));
            }

            if (await UserExists(request.Email))
            {
                return new ConflictObjectResult(ErrorResult.ErrorCodeReturner("Email já existe"));
            }

            if (!ValidEmail(request.Email))
            {
                return new BadRequestObjectResult(ErrorResult.ErrorCodeReturner("Email inválido."));
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created("", new { message = "Usuário criado com sucesso." });
        }

        [HttpDelete("{userID}")]
        public async Task<IActionResult> DeleteUser(int userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return new NotFoundObjectResult(ErrorResult.ErrorCodeReturner("Impossível deletar usuário inexistente."));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        private bool ValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
