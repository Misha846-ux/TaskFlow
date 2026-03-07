using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        //========================================Get=============================================
        [HttpGet]
        public Task<IActionResult> GetAll()
        {
            return null;
        }

        /// <summary>
        /// Позволяет получать пользователей порционно
        /// </summary>
        /// <param name="count">Количество пользователей в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <returns></returns>
        [HttpGet("Filtred")]
        public Task<IActionResult> GetPagination([FromQuery] int count, int side)
        {
            return null;
        }

        /// <summary>
        /// Позволяет получить порцию пользователей с буквосочетанием в имени.
        /// </summary>
        /// <param name="count">Количество пользователей в одной порции данных</param>
        /// <param name="side">Номер порции данных</param>
        /// <param name="name">Буквосочетание которое должно присутствовать в имени</param>
        /// <returns></returns>
        [HttpGet("Filtred/SearchByName")]
        public Task<IActionResult> GetByName([FromQuery] int count, int side, string name)
        {
            return null;
        }

        /// <summary>
        /// Возращяет всех пользователей у которых есть данное буквосочетание в имени.
        /// </summary>
        /// <param name="name">Буквосочетание которое должно присутствовать в имени</param>
        /// <returns></returns>
        [HttpGet("SearchByName")]
        public Task<IActionResult> GetByName([FromRoute] string name)
        {
            return null;
        }

        [HttpGet("ById")]
        public Task<IActionResult> GetById([FromRoute] int id)
        {
            return null;
        }

        [HttpGet("ByEmail")]
        public Task<IActionResult> GetByEmail([FromRoute] string rout)
        {
            return null;
        }



        //============================================Delete===============================================

        /// <summary>
        /// Роут для админов позволяет удалить любого пользователя по id
        /// </summary>
        /// <param name="id">id пользователя который будет удалён</param>
        /// <returns></returns>
        [HttpDelete("DeleteForAdmin")]
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> DeleteForAdmin([FromRoute] int id)
        {
            return null;
        }

        /// <summary>
        /// Удаляет пользователя беря id из Access или Refresh токена
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public Task<IActionResult> Delete()
        {
            return null;
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
        public Task<IActionResult> UpdateForAdmin([FromBody] UserUpdateDto newUser)
        {
            return null;
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
        public Task<IActionResult> Update([FromBody] UserUpdateDto newUser)
        {
            return null;
        }
    }
}
