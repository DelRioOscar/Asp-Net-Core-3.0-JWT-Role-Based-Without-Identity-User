using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserBackend.Entities;
using UserBackend.Interfaces;
using UserBackend.Models;
using UserBackend.Tools;

namespace UserBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, ITemplateCrud<User>
    {
        private readonly UserDbContext db;

        public UsersController(UserDbContext context)
        {
            db = context;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Create([FromBody] User item)
        {
            if(!db.Users.Any(u => u.Email == item.Email))
            {
                db.Add(item);
                db.SaveChanges();
                return Ok(item);
            }
            return BadRequest(new Status(400, "No se puede agregar porque el email existe."));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Edit(int id, [FromBody] User item)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(db.Users.Include(r => r.Rol).ToList());
        }
    }
}