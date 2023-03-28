using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Net.Mail;
using PhotoPicker.Entities;
using PhotoPicker.Infrastructure.Database;
using PhotoPicker.Infrastructure.Http;

namespace PhotoPicker.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : BaseController
    {
        private readonly DataContext _dataContext;
        public AccountController(DataContext dataContext) {  _dataContext = dataContext; }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var user = await _dataContext.Set<User>()
                             .Where(x => x.Email == req.Email)
                             .FirstOrDefaultAsync();

                // User does not exist
                if (user is null)
                    return BadRequest("Invalid email or password.");

                // Verify password
                var hasher = new PasswordHasher<User>();
                if (hasher.VerifyHashedPassword(user, user.HashedPassword, req.Password) == PasswordVerificationResult.Failed)
                    return BadRequest("Invalid email or password.");

                var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                user.LastLogin = DateTime.Now;
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Singup([FromBody] SignupRequest req)
        {
            try
            {
                // Validate email
                try
                {
                    _ = new MailAddress(req.Email);
                }
                catch
                {
                    return BadRequest("Invalid email.");
                }

                // Check if email is unique
                if (await _dataContext.Set<User>().Where(x => x.Email == req.Email).CountAsync() > 0)
                    return BadRequest("Email already in use.");

                // Create new user
                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    Name = req.Name,
                    Email = req.Email,
                    HashedPassword = hasher.HashPassword(new(), req.Password),
                    Role = req.Role
                };

                // Save to DB
                await _dataContext.AddAsync(user);
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }

    #region Controller DTOs
    #pragma warning disable CS8618 // Suppress "Non-nullable field must contain a non-null value when exiting constructor." warning
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class SignupRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserRole Role { get; set; } 
    }
    #pragma warning restore CS8618
    #endregion
}
