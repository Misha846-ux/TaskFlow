using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.ProjectDTOs.CommonDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.CreateProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.DetailsProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.UpdateProjectDTOs;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    //[Route("api/companies/{companyId}/projects")] - как вариант для дальнейшего использования.
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        /// <summary>
        /// Возвращает идентификатор текущего пользователя из JWT-токена.
        /// </summary>
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        /// <summary>
        /// Создаёт новый проект.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto dto, CancellationToken cancellationToken)
        {
            var id = await _service.CreateProjectAsync(dto, cancellationToken);
            return Ok(id);
        }

        /// <summary>
        /// Возвращает проект по Id.
        /// Доступно только пользователям.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id, CancellationToken cancellationToken)
        {
            var result = await _service.GetProjectByIdAsync(id, GetUserId(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает проект по Id без ограничений доступа.
        /// Для Админа.
        /// </summary>
        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectAdmin(int id, CancellationToken cancellationToken)
        {
            var result = await _service.GetProjectByIdAdminAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает список проектов текущего пользователя.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] ProjectFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await _service.GetProjectsAsync(filter, GetUserId(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает список всех проектов.
        /// Для Админа.
        /// </summary>
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectsAdmin([FromQuery] ProjectFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await _service.GetProjectsAdminAsync(filter, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Выполняет поиск проектов по названию.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> GetByName(string name, int page, int pageSize, CancellationToken cancellationToken)
        {
            var result = await _service.GetProjectsByNameAsync(name, GetUserId(), page, pageSize, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Выполняет поиск проектов по названию среди всех проектов.
        /// Для Админа.
        /// </summary>
        [HttpGet("admin/search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByNameAdmin(string name, int page, int pageSize, CancellationToken cancellationToken)
        {
            var result = await _service.GetProjectsByNameAdminAsync(name, page, pageSize, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Обновляет данные проекта.
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto dto, CancellationToken cancellationToken)
        {
            await _service.UpdateProjectAsync(id, dto, GetUserId(), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Обновляет данные проекта без проверки доступа.
        /// Для Админа.
        /// </summary>
        [HttpPatch("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProjectAdmin(int id, UpdateProjectDto dto, CancellationToken cancellationToken)
        {
            await _service.UpdateProjectAdminAsync(id, dto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Удаляет проект.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteProjectAsync(id, GetUserId(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Удаляет проект без проверки доступа.
        /// Для Админа.
        /// </summary>
        [HttpDelete("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProjectAdmin(int id, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteProjectAdminAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
