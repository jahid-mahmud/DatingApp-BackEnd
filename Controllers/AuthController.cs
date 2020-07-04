using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._config = config;
            this._repo = repo;

        }

        [HttpPost("register")]

        public async Task<IActionResult> Resister(UserForResisterDto userForResisterDto)
        {
            userForResisterDto.UserName = userForResisterDto.UserName.ToLower();
            if (await _repo.UserExist(userForResisterDto.UserName))
            {
                return BadRequest("Username already exists");
            }
            var userToCreate = new User
            {
                UserName = userForResisterDto.UserName
            };
            var createdUser = await _repo.Resiter(userToCreate, userForResisterDto.Password);
            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();

            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSetting:Token").Value));
            var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken (tokenDescriptor);
            return Ok( new{
                token = tokenHandler.WriteToken (token)
            });

        }

    }
}