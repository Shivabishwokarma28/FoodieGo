using FoodieGo_Backend.Data;
using FoodieGo_Backend.DTOs;
using FoodieGo_Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodieGo_Backend.Controllers
{
    // [ApiController] enables features like automatic 400 errors if the DTO is missing required fields.
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        // --- REGISTER NEW USER ---
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto reg)
        {
            // 1. Validation: Ensure the email isn't already in the database
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == reg.Email);
            if (existingUser != null)
            {
                return BadRequest("Email Already Exist");
            }

            // 2. Mapping: Create a new User entity from the DTO
            var user = new User
            {
                Name = reg.Name,
                Email = reg.Email,
                Role = "Customer" // Default role for all new sign-ups
            };

            // 3. Security: Hash the password. Never store plain-text passwords!
            // PasswordHasher adds "salt" automatically to prevent rainbow table attacks.
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, reg.Password);

            // 4. Persistence: Save the user to the SQL database
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok("User Register Successfully!");
        }

        // --- LOGIN USER ---
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto log)
        {
            // 1. Find User: Check if the email exists in our records
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == log.Email);
            if (user == null)
            {
                // We return "Invalid Credentials" rather than "User not found" for security
                return Unauthorized("Invalid Credentials");
            }

            // 2. Password Check: Compare the incoming password with the stored hash
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, log.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid Credentials");
            }

            // 3. Token Generation: If password is correct, create a JWT
            var token = GenerateJwtToken(user);

            // Return the token as a JSON object: { "token": "..." }
            return Ok(new { token });
        }

        // --- GENERATE JWT TOKEN ---
        // This creates a signed string that the Frontend can use for future requests.
        private object GenerateJwtToken(User user)
        {
            // Claims are pieces of data about the user encoded inside the token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserName", user.Name) // Custom claim to show user name on the frontend
            };

            // Create a security key using the secret string in appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            // Creds define the algorithm used to sign the token (HMAC SHA256)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Assemble the token components
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token becomes invalid after 2 hours
                signingCredentials: creds
            );

            // Convert the token object into a final "Base64" string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}