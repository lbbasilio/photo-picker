using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using PhotoPicker.Entities;
using Microsoft.EntityFrameworkCore;
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
    }

    public class LoginRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
