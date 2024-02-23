using khdima.dbContext;
using khdima.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace khdima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        // appelle le dbconext pour l'utiliser comme outile pour la base de donnee
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public Auth(AppDbContext context , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Sign-in", Name = "Sign-in")]
        public async Task<IActionResult> SingIn([FromBody] SignInRequest signInRequest)
        {
              /*  {
                    "email": "r.aitelbacha@gmail.com",
                    "password": "red"
                  }
              */

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == signInRequest.Email);
            
            if (user == null)
            {
                return NotFound("Aucun utilisateur trouvé avec cette adresse e-mail.");
            }

         
            if (user.password != signInRequest.Password)
            {
                return Unauthorized("Mot de passe incorrect.");
            }

            var users = await _context.Users.Select(u => new
            {
                u.id,
                u.first_name,
                u.last_name,
                u.phone,
                u.birthday_date,
                u.email,
                u.last_access,
                u.first_access,
                u.created_at,
                u.updated_at
            }
            ).ToListAsync();

            // Générer un JWT
            var token = GenerateJwtToken(user);

            return Ok(token);

         
        }

        private string GenerateJwtToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                       claims: new[] {
                    new Claim("idUser", user.id.ToString()),
                    new Claim("name", $"{user.first_name} {user.last_name}"),
                },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);


           return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
