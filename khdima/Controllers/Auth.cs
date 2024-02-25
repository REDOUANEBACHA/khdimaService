using khdima.dbContext;
using khdima.Helpers;
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
        private readonly Security _security;
        public Auth(AppDbContext context , IConfiguration configuration , Security security)
        {
            _context = context;
            _configuration = configuration;
            _security = security;
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

            var users = await _context.Users.ToListAsync();

            // Générer un JWT
            var token = _security.GenerateJwtToken(user);
    

            // Écriture du token dans le cookie
            Response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,// Assurez-vous que le cookie peut être lu depuis JavaScript si nécessaire
                Secure  =  true,
                SameSite = SameSiteMode.None
            });

            return Ok("succes login");

         
        }
  

    }
}
