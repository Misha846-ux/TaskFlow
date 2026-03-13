using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.AuthDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthorizationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> CreateAccaunt([FromBody] UserPostDto user)
        {
            var result = await _authService.RegisterAsync(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            var result = await _authService.LoginAsync(user);
            return Ok(result);
        }

        /// <summary>
        /// Обновление AccessToken с помощью RefreshToken.
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenRequestDto request)
        {
            var tokens = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(tokens);
        }

        [HttpPost]
        public Task<IActionResult> ForgotPassword()
        {
            return null;
        }

    }
}