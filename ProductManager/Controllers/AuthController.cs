using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PM.Models;

namespace ProductManager.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // User Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully.");
        }

        // User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid credentials.");

            return Ok("Login successful.");
        }

        [HttpGet("check")]
        public IActionResult CheckAuthentication()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Ok(new { isAuthenticated = true });
            }
            return Unauthorized(new { isAuthenticated = false });
        }
    }
}
