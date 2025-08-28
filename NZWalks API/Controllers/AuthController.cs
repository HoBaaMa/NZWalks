using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        // POST: /api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            if (result.Succeeded)
            {
                // Add Roles to This User
                //await _userManager.AddToRoleAsync(identityUser, registerDto.Roles);
                // Add multiple roles to this user
                if (registerDto.Roles != null && registerDto.Roles.Length > 0)
                {
                    foreach (var role in registerDto.Roles)
                    {
                        await _userManager.AddToRoleAsync(identityUser, role);
                    }
                    return Ok("User registered sucessfully");
                }
            }
            return BadRequest();
        }

        // POST: /api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);
            if (user is null) return BadRequest();

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (isPasswordCorrect)
            {
                // Get Roles For The User
                var roles = await _userManager.GetRolesAsync(user);
                if (roles is not null)
                {
                    // Create Token
                    var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    };

                    return Ok(response);
                }
            }
            return BadRequest();
        }
    }
}
