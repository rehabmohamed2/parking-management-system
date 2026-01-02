using Microsoft.AspNetCore.Mvc;
using IntegrationServices.DTOs;
using IntegrationServices.services;
using IntegrationServices.DataBase;
using Microsoft.EntityFrameworkCore;

namespace IntegrationServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        public AuthController(ILogger<AuthController> logger, JwtService jwtService, AppDbContext context)
        {
            _logger = logger;
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        
        {
            Console.WriteLine("Login attempt for email: " + request.Email);
            try
            {
                // Find user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password" });
                }

                // Verify password using BCrypt
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return Unauthorized(new { Message = "Invalid email or password" });
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(user.Email);

                return Ok(new
                {
                    Message = "Login successful",
                    Token = token,
                    Email = user.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
