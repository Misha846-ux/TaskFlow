using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController: ControllerBase
    {
        [HttpPost("SingUp")]
        public Task<IActionResult> CreateAccaunt([FromBody]UserPostDto user)
        {
            return null;
        }

        [HttpPost]
        public Task<IActionResult> Login([FromBody]UserLoginDto user)
        {
            return null;
        }
        [HttpPost]
        public Task<IActionResult> RefreshAccessToken()
        {
            return null;
        }
        [HttpPost]
        public Task<IActionResult> ForgotPassword()
        {
            return null;
        }

    }
}
