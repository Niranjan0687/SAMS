using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsAPI.Data;
using SmsAPI.DTOs;
using SmsAPI.Services;
using System.Security.Claims;

namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly TokenService _tokenService;
        private readonly AdmissionDbContext _context;
        public AuthController(TokenService tokenService, AdmissionDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("Username and password are required");
            }

            var user = await _context.Users
                .Where(u => u.UserName != null)  // Filter out null usernames first
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == login.Username.ToLower());

            if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            user.RefreshToken = refreshToken;
            await _context.SaveChangesAsync();
            
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        protected bool VerifyPassword(string password,string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
