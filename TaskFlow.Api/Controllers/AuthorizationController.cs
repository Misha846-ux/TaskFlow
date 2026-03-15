using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.AuthDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Services;

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

        [HttpPost("LogIn")]
        public Task<IActionResult> Login([FromBody]UserLoginDto user)
        {
            return null;
        }
        [HttpPost("Refresh")]
        public Task<IActionResult> RefreshAccessToken()
        {
            return null;
        }

        /// <summary>
        /// Отправляет на почту введённую пользователем сообщение с кодом востановления акаунта. Если акаунта с такой почтой 
        /// нет то выдаёт ошибку.
        /// </summary>
        /// <returns></returns>
        [HttpPut("ForgotPassword/GetToken{email}")]
        public Task<IActionResult> ForgotPassword([FromRoute] string email)
        {
            return null;
        }

        /// <summary>
        /// Позволяет пользователю зарегаться на сайте через код востановления акаунта а не пароль.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword/LoginWithToken")]
        public Task<IActionResult> ForgotPasswordLogin([FromBody] UserLoginDto userLogin)
        {
            return null;
        }

    }
}