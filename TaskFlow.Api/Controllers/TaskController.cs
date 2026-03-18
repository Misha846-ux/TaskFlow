using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDto;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Services;
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
        [HttpGet("admin/GetAll")]
        public async Task<IActionResult> GetAllTasks(CancellationToken cancellationToken, [FromQuery] int pagenum = 1, [FromQuery] int limit = 10)
        {
            var tasks = await _taskService.GetAllTasksAsync(cancellationToken);
            var chunk = tasks
            .Skip((pagenum - 1) * limit)
                .Take(limit);
            return Ok(chunk);
        }
        /// <summary>
        /// Allow get datas by pagination
        /// </summary>
        /// <param name="count">Amount of tasks in one datas portion</param>
        /// <param name="side">Number of data's portion</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Filtred")]
        public async Task<IActionResult> GetPagination([FromQuery] int count, int side, CancellationToken cancellationToken)
        {
            ICollection<TaskGetDto> tasks = await _taskService.GetTasksPaginationAsync(count, side, cancellationToken);
            return Ok(tasks);
        }
        /// <summary>
        /// Allow get datas by pagination
        /// </summary>
        /// <param name="projectId">roject id for more corect portion</param>
        /// <param name="count">Amount of tasks in one datas portion</param>
        /// <param name="side">Number of data's portion</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("FiltredWithProjectId")]
        public async Task<IActionResult> GetExtraPagination(int projectId,[FromQuery] int count, int side, CancellationToken cancellationToken)
        {
            ICollection<TaskGetDto> tasks = await _taskService.GetProjectTasksPaginationAsync(projectId,count, side, cancellationToken);
            return Ok(tasks);
        }
        /// <summary>
        /// Allow get data's name by pagination and name read in letters
        /// </summary>
        /// <param name="count">Allow get datas by pagination</param>
        /// <param name="side">Number of data's portion</param>
        /// <param name="name">Letter's reader which has to be in name</param>
        /// <returns></returns>
        [HttpGet("Filtred/SearchByName")]
        public async Task<IActionResult> GetByNamePagination([FromQuery] int count, int side, string name, int projectId, CancellationToken cancellationToken)
        {
            ICollection<TaskGetDto> tasks = await _taskService.GetTasksByNamePaginationAsync(name, projectId,count, side, cancellationToken);
            return Ok(tasks);
        }
        /// <summary>
        /// Method for getting Tasks by id, uses for admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.Admin))]
        [HttpGet("admin/GetById/{id}")]
        public async Task<IActionResult> GetTaskById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetTaskByIdAsync(id, cancellationToken);
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
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetTaskByName(CancellationToken cancellationToken, [FromRoute] string name, [FromQuery] int projectId)
        {
            var tasks = await _taskService.GetTaskByNameAsync(name, projectId, cancellationToken);
            return Ok(tasks);
        }

        /// <summary>
        /// Method for getting Tasks by deadline
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetByDate/{date}")]
        public async Task<IActionResult> GetTaskByDeadLine([FromQuery] int projectId, [FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetTaskByDeadLineAsync(date, projectId, cancellationToken); 
            return Ok(task);
        }
        [Authorize]
        [HttpGet("GetByStatus/{status}")]
        public async Task<IActionResult> GetTaskByStatus([FromRoute] TaskFlow.Domain.Enums.TaskEnums.TaskStatus status, [FromQuery] int projectId, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetTaskByStatusAsync(status, projectId, cancellationToken);
            return Ok(task);
        }
        /// <summary>
        /// Method for getting all Tasks, uses for user
        /// </summary>
        /// <param name="pagenum"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(GlobalRole.User))]
        [HttpGet("user/GetAll")]
        public async Task<IActionResult> GetAllUserTasks(CancellationToken cancellationToken, [FromQuery] int pagenum = 1, [FromQuery] int limit = 10)
        {
            var tasks = await _taskService.GetAllTasksAsync(cancellationToken);
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
        [HttpGet("user/GetById/{id}")]
        public async Task<IActionResult> GetUserTaskById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetTaskByIdAsync(id, cancellationToken);
            return Ok(task);
        }

        //======================================================================POST=============================================================================

        /// <summary>
        /// Method for adding Tasks 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] TaskPostDto dto, CancellationToken cancellationToken)
        {
            int? id = await _taskService.CreateTaskAsync(dto, cancellationToken);
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
        [HttpPut("admin/UpdateById/{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _taskService.UpdeteTaskAsync(id, dto, cancellationToken);

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
        [HttpPut("user/UpdateById/{id}")]
        public async Task<IActionResult> UpdateUserTask(int id, TaskUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _taskService.UpdeteTaskAsync(id, dto, cancellationToken);

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
        [HttpDelete("admin/DeleteById/{id}")]
        public async Task<IActionResult> DeleteTaskById(int id, CancellationToken cancellationToken)
        {
            var success = await _taskService.DeleteTaskByIdAsync(id, cancellationToken);

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
        [HttpDelete("user/DeleteById/{id}")]
        public async Task<IActionResult> DeleteUserTaskById(int id, CancellationToken cancellationToken)
        {
            var success = await _taskService.DeleteTaskByIdAsync(id, cancellationToken);

            if (!success)
                return NotFound();

            return NoContent();
        }

        
    }
}
