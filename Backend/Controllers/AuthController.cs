using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Context;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ES2DBContext _context;

        public AuthController(IConfiguration configuration, ES2DBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] AuthModel login)
        {
            if (IsValidUser(login))
            {
                var token = GenerateJwtToken(login.Username, login.Id);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private bool IsValidUser(AuthModel login)
        {
            try
            {
                var utilizador = _context.Utilizadors.SingleOrDefault(u => u.Username == login.Username);

                if (utilizador != null)
                {
                    if (utilizador.Senha == login.Password)
                    {
                        login.Id = utilizador.Id;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar as credenciais do usuário: {ex.Message}");
            }

            return false;
        }

        private string GenerateJwtToken(string username, Guid idUtilizador)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("idUtilizador", idUtilizador.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenExpirationTimeInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
