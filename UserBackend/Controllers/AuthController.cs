using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserBackend.Dtos;
using UserBackend.Models;
using UserBackend.Tools;

namespace UserBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserDbContext db;

        public AuthController(UserDbContext context)
        {
            db = context;
        }
        
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] UserDto user)
        {
            if (db.Users.Any(u => u.Email == user.Email))
            {
                User userRequest = db.Users.Include(r => r.Rol).SingleOrDefault(u => u.Email == user.Email);
                if (new PasswordHasher().Check(userRequest.Password, user.Password))
                {
                    // Construir el token...
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("xWesRRRDVTyff7m895NtJ35o7X2WZucXEKxWesR456RRDVTyff7mNtJ35o7X2WZucXEK");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                            new Claim(ClaimTypes.NameIdentifier, userRequest.Id.ToString()),
                            new Claim(ClaimTypes.Name, userRequest.Name),
                            new Claim(ClaimTypes.Role, userRequest.Rol.Name)
                        }),

                        Expires = DateTime.UtcNow.AddMinutes(60),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Ok(new {
                        token = tokenHandler.WriteToken(token),
                        userRequest.Rol.Name,
                        token.ValidTo
                    });
                }
            }
            return BadRequest(new Status(400, "Email y/o contraseña incorrecta."));
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (!db.Users.Any(u => u.Email == user.Email))
            {
                user.Password = new PasswordHasher().Hash(user.Password);
                db.Add(user);
                db.SaveChanges();
                return Ok();
            }
            return BadRequest(new Status(400, "No se puede agregar porque el email existe."));

        }
    }
}