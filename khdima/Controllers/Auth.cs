using khdima.dbContext;
using khdima.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using khdima.Models.type;

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
            var userResult = (from userHasRole in _context.User_has_role
                              join user in _context.Users on userHasRole.id_user equals user.id
                              join role in _context.Roles on userHasRole.id_role equals role.id
                              where user.email == signInRequest.Email
                              select new UserRole
                              {
                                  UserHasRole = userHasRole,
                                  User = user,
                                  Role = role
                              }).FirstOrDefault();

            var filterUserResult = new { idUser =  userResult.User.id , roleUser = userResult.Role.title };
            if (userResult == null)
            {
                return NotFound("Aucun utilisateur trouvé avec cette adresse e-mail.");
            }

         
            if (userResult.User.password != signInRequest.Password)
            {
                return Unauthorized("Mot de passe incorrect.");
            }

         
            // Générer un JWT
            var token = _security.GenerateJwtToken(userResult);


            // Écriture du token dans le cookie
            Response.Cookies.Append("AccessToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure  =  true,
                SameSite = SameSiteMode.None
            });

            return Ok(filterUserResult);
         

         
        }
  

    }
}
