using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController(IUserService _userService): ControllerBase
    {
        //========================================Get=============================================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<UserGetDto> users = await _userService.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Позволяет получать пользователей порционно
        /// </summary>
        /// <param name="count">Количество пользователей в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <returns></returns>
        [HttpGet("Filtred")]
        public async Task<IActionResult> GetPagination([FromQuery] int count, int side, CancellationToken cancellationToken)
        {
            ICollection<UserGetDto> users = await _userService.GetUsersPagitationAsync(count, side, cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Позволяет получить порцию пользователей с буквосочетанием в имени.
        /// </summary>
        /// <param name="count">Количество пользователей в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <param name="name">Буквосочетание которое должно присутствовать в имени</param>
        /// <returns></returns>
        [HttpGet("Filtred/SearchByName")]
        public async Task<IActionResult> GetByNamePagination([FromQuery] int count, int side, string name, CancellationToken cancellationToken)
        {
            ICollection<UserGetDto> users = await _userService.GetUsersByNamePagitationAsync(name, count, side, cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Возращяет всех пользователей у которых есть данное буквосочетание в имени.
        /// </summary>
        /// <param name="name">Буквосочетание которое должно присутствовать в имени</param>
        /// <returns></returns>
        [HttpGet("SearchByName{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name, CancellationToken cancellationToken)
        {
            ICollection<UserGetDto> users = await _userService.GetUsersByNameAsync(name, cancellationToken);
            return Ok(users);
        }

        [HttpGet("ById{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            UserGetDto user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [HttpGet("ByEmail{rout}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email, CancellationToken cancellationToken)
        {
            UserGetDto user = await _userService.GetUserByEmailAsync(email, cancellationToken);
            return Ok(user);
        }



        //============================================Delete===============================================

        /// <summary>
        /// Роут для админов позволяет удалить любого пользователя по id
        /// </summary>
        /// <param name="id">id пользователя который будет удалён</param>
        /// <returns></returns>
        [HttpDelete("DeleteForAdmin{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteForAdmin([FromRoute] int id, CancellationToken cancellationToken)
        {
            int? deletedId = await _userService.DeleteUserByIdAsync(id, cancellationToken);
            return Ok(deletedId);
        }

        /// <summary>
        /// Удаляет пользователя беря id из Access или Refresh токена
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userService.DeleteUserByIdAsync(userId, cancellationToken);
            return Ok();
        }


        //=============================================Update=================================================

        /// <summary>
        /// Роут для админов позволяет изменить данные любого пользователя
        /// </summary>
        /// <param name="newUser">Новый юзер id которогу совпадает с тем которого надо изменить. 
        /// Поля значение которых будет null изменению не подвергнутся и будут сохранены</param>
        /// <returns></returns>
        [HttpPut("UpdateForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateForAdmin([FromBody] UserUpdateDto newUser, CancellationToken cancellationToken)
        {
            UserGetDto user = await _userService.UpdateUserForAdminAsync(newUser, cancellationToken);
            return Ok(user);
        }

        /// <summary>
        /// Роут для того чтоб пользователь мог обновить свой профиль. Перед выполнением операции проверяет по 
        /// Access или Refresh токену совпадает ли id юзера которого он хечет изменить с его собственным и если нет то
        /// бракует операцию. То есть роут не должен позволять пользователю менять не себя.
        /// </summary>
        /// <param name="newUser">Новый юзер id которогу совпадает с тем которого надо изменить. 
        /// Поля значение которых будет null изменению не подвергнутся и будут сохранены</param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto newUser, CancellationToken cancellationToken)
        {
            newUser.Id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            UserGetDto user = await _userService.UpdateUserForAdminAsync(newUser, cancellationToken);
            return Ok(user);
        }
    }
}
