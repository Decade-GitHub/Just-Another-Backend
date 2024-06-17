using AccessControlAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AccessControlAPI.Controllers
{
    [ApiController]
    [Route("api/cep")]
    public class CEPController(ApplicationDbContext context, HttpClient httpClient/*, JwtTokenValidator tokenValidator*/) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly HttpClient _httpClient = httpClient;
        // private readonly JwtTokenValidator _tokenValidator = tokenValidator;

        [HttpGet]
        public async Task<IActionResult> GetCEP(/*[FromHeader(Name = "Authorization")] string authorizationHeader,*/ string IdCEP)
        {
            /*
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized();
            }

            ClaimsPrincipal principal;
            try
            {
                principal = _tokenValidator.ValidateToken(authorizationHeader);
            }
            catch (Exception _)
            {
                return Unauthorized($"Failed to validate JWT token: {_.Message}");
            }

            string? UE = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(UE) )
            {
                return Unauthorized("JWT does not possess valid data.");
            }

            var EU = await _context.Users.FirstOrDefaultAsync(u => u.Email == UE);
            if (EU == null)
            {
                return Unauthorized("Unauthorized user.");
            }
            */
            if (string.IsNullOrEmpty(IdCEP) || !IsCEPValid(IdCEP))
            {
                return BadRequest("CEP is null or invalid.");
            }

            var ExisitingCEP = await _context.CEPs.FindAsync(IdCEP);
            if (ExisitingCEP != null)
            {
                return Ok(ExisitingCEP);
            }

            var APIUrl = $"https://api.postmon.com.br/v1/cep/{IdCEP}";
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync(APIUrl);
            }
            catch (Exception _)
            {
                return StatusCode(500, $"Error fetching data from API: {_.Message}");
            }

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Error fetching data from API: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            CEP apiResponse;

            try
            {
                apiResponse = JsonConvert.DeserializeObject<CEP>(content);
            }
            catch (JsonException _)
            {
                return StatusCode(500, $"Error parsing API response: {_.Message}");
            }

            if (apiResponse == null)
            {
                return BadRequest("Invalid data received from API.");
            }

            var cep = new CEP
            {
                IdCEP = IdCEP,
                Cidade = apiResponse.Cidade,
                Logradouro = apiResponse.Logradouro,
                Bairro = apiResponse.Bairro,
                DataInclusao = apiResponse.DataInclusao ?? DateTime.UtcNow
            };
            _context.CEPs.Add(cep);
            await _context.SaveChangesAsync();

            return Ok(cep);
        }
        private static bool IsCEPValid(string CEP)
        {
            string Pattern = @"^\d{8}$";

            if (Regex.IsMatch(CEP, Pattern))
            {
                return true;
            }
            return false;
        }
    }
}