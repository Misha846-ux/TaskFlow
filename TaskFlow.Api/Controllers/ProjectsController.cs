using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found.");
            }
            return userId;
        }

        /// <summary>
        /// Create a new project (admin).
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectPostDto dto, CancellationToken cancellationToken)
        {
            var projectId = await _service.CreateProjectAsync(dto, cancellationToken);
            if (projectId == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = projectId }, new { id = projectId });
        }

        /// <summary>
        /// Get all projects (admin).
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var projects = await _service.GetAllProjectsAsync(cancellationToken);
            return Ok(projects);
        }

        /// <summary>
        /// Get projects portion (admin).
        /// </summary>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("filtered")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPagination([FromQuery] int count, [FromQuery] int side, CancellationToken cancellationToken)
        {
            var projects = await _service.GetProjectsPaginationAsync(count, side, cancellationToken);
            return Ok(projects);
        }

        /// <summary>
        /// Get project by id (admin).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var project = await _service.GetProjectByIdAsync(id, cancellationToken);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        /// <summary>
        /// Get project by name (admin).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("by-name/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var project = await _service.GetProjectByNameAsync(name, cancellationToken);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        /// <summary>
        /// Get projects by company id (admin).
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("by-company/{companyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByCompanyId(int companyId, CancellationToken cancellationToken)
        {
            var projects = await _service.GetProjectsByCompanyIdAsync(companyId, cancellationToken);
            return Ok(projects);
        }

        /// <summary>
        /// Get projects by company id with pagination (admin).
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("by-company/{companyId}/filtered")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByCompanyIdPagination(int companyId, [FromQuery] int count, [FromQuery] int side, CancellationToken cancellationToken)
        {
            var projects = await _service.GetProjectsByCompanyIdPaginationAsync(companyId, count, side, cancellationToken);
            return Ok(projects);
        }

        /// <summary>
        /// Get projects of user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("my-projects")]
        public async Task<IActionResult> GetUserProjects(CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            var projects = await _service.GetUserProjectsAsync(userId, cancellationToken);
            return Ok(projects);
        }

        /// <summary>
        /// Get projects of user with pagination.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("my-projects/filtered")]
        public async Task<IActionResult> GetUserProjectsPagination([FromQuery] int count, [FromQuery] int side, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            var projects = await _service.GetUserProjectsPaginationAsync(userId, count, side, cancellationToken);
            return Ok(projects);
        }

        /// <summary>
        /// Get users of project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{projectId}/users")]
        public async Task<IActionResult> GetProjectUsers(int projectId, CancellationToken cancellationToken)
        {
            var users = await _service.GetProjectUsersAsync(projectId, cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Add user to project (admin).
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{projectId}/users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToProject(int projectId, int userId, CancellationToken cancellationToken)
        {
            var result = await _service.AddUserToProjectAsync(projectId, userId, cancellationToken);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(new { id = result });
        }

        /// <summary>
        /// Remove user from project (admin).
        /// </summary>
        /// <param name="projectUserId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{projectUserId}/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserFromProject(int projectUserId, CancellationToken cancellationToken)
        {
            var result = await _service.RemoveUserFromProjectAsync(projectUserId, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new { id = result });
        }

        /// <summary>
        /// Update project (admin).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateProjectAsync(id, dto, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new { id = result });
        }

        /// <summary>
        /// Delete project by id (admin).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteProjectAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new { id = result });
        }
    }
}
