using Microsoft.AspNetCore.Mvc;
using TrainingCenter.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration; // Add this to access IConfiguration

namespace TrainingCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly string adminEmail = "Admin@Admin.com";
        private readonly string adminPassword = "Admin@123";
        private readonly IConfiguration _configuration; // Use IConfiguration instead of WebApplicationBuilder

        // Inject IConfiguration directly
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            string email = loginDTO.Email;
            string password = loginDTO.Password;

            if (email != adminEmail || password != adminPassword) // Note: Use || instead of &&
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
