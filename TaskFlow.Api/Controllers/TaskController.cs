using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDto;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(ITaskService _taskService):ControllerBase
    {
        //======================================================================GET==============================================================================

        /// <summary>
        /// Method for getting all Tasks, uses for admin
        /// </summary>
        /// <param name="pagenum"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.Admin))]
        [HttpGet("admin/get")]
        public async Task<IActionResult> GetAllTasks([FromQuery] int pagenum = 1, [FromQuery] int limit = 10)
        {
            var tasks = await _taskService.GetAllTasksAsync();
            var chunk = tasks
            .Skip((pagenum - 1) * limit)
                .Take(limit);
            return Ok(chunk);
        }

        /// <summary>
        /// Method for getting Tasks by id, uses for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.Admin))]
        [HttpGet("admin/get/{id}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }

        /// <summary>
        /// Method for getting Tasks by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pagenum"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get/{name}")]
        public async Task<IActionResult> GetTaskByName([FromRoute] string name, [FromQuery] int pagenum = 1, [FromQuery] int limit = 10)
        {
            var tasks = await _taskService.GetTaskByNameAsync(name, 100); // ВРЕМЕНННОЕ ЗНАЧЕНИЕ ПОТОМ ЗАМЕНИТЬ НА ИД ПРОЕКТА
            Console.WriteLine("ВРЕМЕННОЕ ЗНАЧЕНИЕ ПОТОМ ЗАМЕНИТЬ НА ИД ПРОЕКТА в методе GetTaskByName. Руслан поработаешь с этим");
            var chunk = tasks
            .Skip((pagenum - 1) * limit)
                .Take(limit);
            return Ok(chunk);
        }

        /// <summary>
        /// Method for getting Tasks by deadline
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get/{date}")]
        public async Task<IActionResult> GetTaskByDeadLine([FromRoute] DateTime date)
        {
            var task = await _taskService.GetTaskByDeadLineAsync(date, 100); //ВРЕМЕННОЕ ЗНАЧЕНИЕ ПОТОМ ЗАМЕНИТЬ НА ИД ПРОЕКТА
            Console.WriteLine("ВРЕМЕННОЕ ЗНАЧЕНИЕ ПОТОМ ЗАМЕНИТЬ НА ИД ПРОЕКТА в методе GetTaskByDeadLine. Руслан поработаешь с этим");
            return Ok(task);
        }

        /// <summary>
        /// Method for getting all Tasks, uses for user
        /// </summary>
        /// <param name="pagenum"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.User))]
        [HttpGet("user/get")]
        public async Task<IActionResult> GetAllUserTasks([FromQuery] int pagenum = 1, [FromQuery] int limit = 10)
        {
            var tasks = await _taskService.GetAllTasksAsync();
            var chunk = tasks
            .Skip((pagenum - 1) * limit)
                .Take(limit);
            return Ok(chunk);
        }

        /// <summary>
        /// Method for getting Tasks by id, uses for user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.User))]
        [HttpGet("user/get/{id}")]
        public async Task<IActionResult> GetUserTaskById([FromRoute] int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }

        //======================================================================POST=============================================================================

        /// <summary>
        /// Method for adding Tasks 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("post")]
        public async Task<IActionResult> AddTask([FromBody] TaskPostDto dto)
        {
            int? id = await _taskService.CreateTaskAsync(dto);
            if (id != null)
            {
                return CreatedAtAction(nameof(GetTaskById), new { id }, id);
            }
            else
            {
                return BadRequest();
            }
        }

        //======================================================================Update===========================================================================

        /// <summary>
        /// Method for updating Tasks by id, uses for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.Admin))]
        [HttpPut("admin/update/{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto dto)
        {
            var result = await _taskService.UpdeteTaskAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Method for updating Tasks by id, uses for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.User))]
        [HttpPut("user/update/{id}")]
        public async Task<IActionResult> UpdateUserTask(int id, TaskUpdateDto dto)
        {
            var result = await _taskService.UpdeteTaskAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        //======================================================================Delete===========================================================================

        /// <summary>
        /// Method for deleting Tasks by id, uses for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.Admin))]
        [HttpDelete("admin/delete/{id}")]
        public async Task<IActionResult> DeleteTaskById(int id)
        {
            var success = await _taskService.DeleteTaskByIdAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Method for deleting Tasks by id, uses for user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.User))]
        [HttpDelete("user/delete/{id}")]
        public async Task<IActionResult> DeleteUserTaskById(int id)
        {
            var success = await _taskService.DeleteTaskByIdAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        
    }
}
