using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;
using System.Text.Json;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, ILogger<AuthController> logger)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
            this._logger = logger;
        }

        // POST: /api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            _logger.LogInformation("User registration attempt for username: {UserName}", registerDto.UserName);

            var identityUser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, registerDto.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully for username: {UserName}", registerDto.UserName);

                // Add Roles to This User
                //await _userManager.AddToRoleAsync(identityUser, registerDto.Roles);
                // Add multiple roles to this user
                if (registerDto.Roles != null && registerDto.Roles.Length > 0)
                {
                    foreach (var role in registerDto.Roles)
                    {
                        await _userManager.AddToRoleAsync(identityUser, role);
                    }
                    _logger.LogInformation("Roles {Roles} assigned to user: {UserName}", string.Join(", ", registerDto.Roles), registerDto.UserName);
                    return Ok("User registered sucessfully");
                }
                _logger.LogWarning("No roles provided for user: {UserName}", registerDto.UserName);
            }
            else
            {
                _logger.LogWarning("User registration failed for username: {UserName}. Errors: {Errors}", 
                    registerDto.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return BadRequest();
        }

        // POST: /api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for username: {UserName}", loginDto.UserName);

            var user = await _userManager.FindByEmailAsync(loginDto.UserName);
            if (user is null) 
            {
                _logger.LogWarning("Login failed - user not found: {UserName}", loginDto.UserName);
                return BadRequest();
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (isPasswordCorrect)
            {
                _logger.LogInformation("Password verified for user: {UserName}", loginDto.UserName);

                // Get Roles For The User
                var roles = await _userManager.GetRolesAsync(user);
                if (roles is not null)
                {
                    _logger.LogInformation("User roles retrieved for {UserName}: {Roles}", loginDto.UserName, string.Join(", ", roles));

                    // Create Token
                    var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    };

                    _logger.LogInformation("JWT token created successfully for user: {UserName}", loginDto.UserName);
                    return Ok(response);
                }
                _logger.LogWarning("No roles found for user: {UserName}", loginDto.UserName);
            }
            else
            {
                _logger.LogWarning("Login failed - incorrect password for user: {UserName}", loginDto.UserName);
            }
            return BadRequest();
        }
    }
}
