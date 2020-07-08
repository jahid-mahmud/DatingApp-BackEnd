using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace DatingApp.API.Controllers

{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        public UsersController(IDatingRepository repo)
        {
            this._repo = repo;

        }
        [HttpGet]
        public async Task<IActionResult> GetUsers() {
            var users = await _repo.GetUsers ();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id) {
            var users = await _repo.GetUser (id);
            return Ok(users);
        }

    }
}