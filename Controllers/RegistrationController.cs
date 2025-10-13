using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmsAPI.Data;
using SmsAPI.Models;

namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly AdmissionDbContext _context;
        public RegistrationController(AdmissionDbContext context)
        {
            _context = context;
        }
        private string HashPassword(string password)
        {
            // Implement a proper hashing mechanism here
            return BCrypt.Net.BCrypt.HashPassword(password);  

        }
        [HttpPost("register")]
        public async Task< IActionResult> Register([FromBody] Users registrationDto)
        {
            //check username already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == registrationDto.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }
            // Validate the incoming registration data
            if (string.IsNullOrEmpty(registrationDto.UserName) || string.IsNullOrEmpty(registrationDto.PasswordHash))
            {
                return BadRequest("Username and password are required.");
            }
            // Hash the password before storing it
            var hashedPassword = HashPassword(registrationDto.PasswordHash);
            // Store the user in the database (this is just a placeholder, implement your own logic)
            // In a real application, you would use a database context to save the user
            var user = new Users
            {
                UserName = registrationDto.UserName,
                PasswordHash = hashedPassword,
                RefreshToken = null,
                RefreshTokenExpiryTime = DateTime.MinValue
            };
            // Simulate saving to the database
           
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully.");
        }
    }
}
