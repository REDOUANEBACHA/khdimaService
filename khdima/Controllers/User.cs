using khdima.dbContext;
using khdima.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace khdima.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class user : ControllerBase
    {
        // appelle le dbconext pour l'utiliser comme outile pour la base de donnee
        private readonly AppDbContext _context;
        public user(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet ("GetAllUser",  Name = "GetAllUser")]
        public  async Task<IActionResult> GetAllUser()
        {
            var users = await _context .Users.Select(u=> new
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

            return Ok(users);
        }

        [HttpPost("AddUser" , Name = "AddUser")]
        public async Task<IActionResult> AddUser(Users user)
        {
            if (user == null)
            {
                return BadRequest("Les données ne peuvent pas être nulles.");
            }

            /*   
            

             obligatoir
             ----------
             {
               "first_name": "John",
               "last_name": "Doe",
               "phone": "123456789",
               "birthday_date": "1990-01-01",
               "email": "john@example.com",
               "password": "password"
            }

            optionnelle 
            ----------
           {
             "id": 0,
             "first_name": "string",
             "last_name": "string",
             "phone": "string",
             "birthday_date": "2024-02-22T13:14:34.969Z",
             "email": "string",
             "password": "string",
             "tmp_password": "string",
             "password_send_date": "2024-02-22T13:14:34.969Z",
             "last_access": "2024-02-22T13:14:34.969Z",
             "first_access": "2024-02-22T13:14:34.969Z",
             "created_at": "2024-02-22T13:14:34.969Z",
             "updated_at": "202-02-22T13:14:34.969Z"
            }

            */

            _context.Users.Add(user);
            await  _context.SaveChangesAsync(); 

            return Ok("Données ajoutées avec succès.");
        }

    }
}
