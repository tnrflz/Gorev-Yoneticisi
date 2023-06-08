using GorevYoneticisiProjesi.Entity;
using GorevYoneticisiProjesi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace GorevYoneticisiProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserContoller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _dbContext;

        public UserContoller(IConfiguration configuration, DatabaseContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var values = _dbContext.Users.ToList();
            return Ok(values);
        }


        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp(Users users)
        {

            _dbContext.Add(users);
            _dbContext.SaveChanges();
            return Ok();

        }
        [AllowAnonymous]
        [HttpPost("signIn")]
        public IActionResult SignIn(SignIn signIn)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == signIn.Username && u.Password == signIn.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(signIn.Username);
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }






}
