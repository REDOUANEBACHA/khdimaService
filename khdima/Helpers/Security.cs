

using khdima.Models;
using khdima.Models.type;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace khdima.Helpers
{
    public class Security
    {

        private readonly IConfiguration _configuration;

        public Security(IConfiguration configuration)
        {
  
            _configuration = configuration;
        }

        public string GenerateJwtToken(UserRole UserRole)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                       claims: new[] {
                    new Claim("idUser", UserRole.User.id.ToString()),
                    new Claim("name", $"{UserRole.User.first_name} {UserRole.User.last_name}"),
                     new Claim("role", $"{UserRole.Role.title}"),
                },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public IEnumerable<Claim> DecodedToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtTokenObject = (JwtSecurityToken)validatedToken;

            var claims = jwtTokenObject.Claims;
            return claims;

        }

     
    }
}
