using erp_api.Data;
using erp_api.Entities;
using erp_api.Models;
using erp_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace erp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // ----------------------------
        // Register a new user
        // ----------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            // Check if username already exists
            if (_context.Users.Any(u => u.UserName == dto.UserName))
                return BadRequest("User already exists");

            // Create a hashed password with salt
            CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            // Create and save user entity
            var user = new User
            {
                UserName = dto.UserName,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = dto.Role,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registered Successfully");
        }

        // ----------------------------
        // Login and generate JWT token
        // ----------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid credentials");

            string token = _jwtService.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")] // Protect this route with JWT
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.Select(u =>

            new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.Role
            }).ToListAsync();

            return Ok(users);
        }

        // ----------------------------
        // Create a secure password hash
        // ----------------------------
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        // ----------------------------
        // Verify the hashed password
        // ----------------------------
        private bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(hash);
        }
    }
}
