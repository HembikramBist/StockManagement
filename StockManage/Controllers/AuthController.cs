using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockManagement.Core.Domain.Entities;
using StockManagement.Infrastructure.Persistence.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly StockDbConnect _context;
        private readonly IConfiguration _config;

        public AuthController(StockDbConnect context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // REGISTER (simple)
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password, string role)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == username))
                return Conflict("Username already exists");

            var exists = await _context.Users
                .AnyAsync(u => u.UserName == username);

            if (exists)
                return Conflict(new { message = "Username already exists" });


            var user = new User
            {
                UserName = username,
                Password = password,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }
    }
}
