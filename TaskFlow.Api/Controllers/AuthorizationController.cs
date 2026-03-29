using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController(IUserService _userService): ControllerBase
    {
        [HttpPost("SingUp")]
        public async Task<IActionResult> CreateAccaunt([FromBody]UserPostDto user, CancellationToken cancellationToken)
        {
            int? userId = await _userService.CreateUserAsync(user, cancellationToken);
            return Created();
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto user, CancellationToken cancellationToken)
        {
            RefreshTokenEntity token = await _userService.LoginWithPasswordAsync(user, cancellationToken);
            Response.Cookies.Append("refreshToken", token.Token, new CookieOptions
            {
                HttpOnly = true,       
                Secure = true,        
                SameSite = SameSiteMode.Strict, 
                Expires = token.Expires
            });
            return Ok();
        }
        /// <summary>
        /// Мы предпологаем что когда пользователь хочет получит AccessToken он в обзательном порядке
        /// находится в какой то компании будь то его базовая или какая то другая. При переходе во вкладку 
        /// другой компании AccessToken становится не валидным и фронтенд должен запросить новый
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAccessToken(int companyId, CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue("refreshToken", out string Refreshtoken);
            string accessToken = await _userService.RefreshAsync(Refreshtoken, companyId, cancellationToken);
            return Ok(accessToken);     
        }

        /// <summary>
        /// Отправляет на почту введённую пользователем сообщение с кодом востановления акаунта. Если акаунта с такой почтой 
        /// нет то выдаёт ошибку.
        /// </summary>
        /// <returns></returns>
        [HttpPut("ForgotPassword/GetToken{email}")]
        public async Task<IActionResult> ForgotPassword([FromRoute] string email, CancellationToken cancellationToken)
        {
            string token = await _userService.CreateRecoveryTokenAsync(email, cancellationToken);
            return Ok(token);
        }

        /// <summary>
        /// Позволяет пользователю зарегаться на сайте через код востановления акаунта а не пароль.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword/LoginWithToken")]
        public async Task<IActionResult> ForgotPasswordLogin([FromBody] UserLoginDto userLogin, CancellationToken cancellationToken)
        {
            RefreshTokenEntity token = await _userService.LoginWithRecoveryTokenAsync(userLogin, cancellationToken);
            Response.Cookies.Append("refreshToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.Expires
            });
            return Ok();
        }

    }
}