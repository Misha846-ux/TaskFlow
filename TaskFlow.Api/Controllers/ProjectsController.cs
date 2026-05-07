using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        /// <summary>
        /// Helper method to get the user ID from the claims.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedException("User ID not found.");
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
        [Authorize(Policy = nameof(CompanyRole.Manager))]
        public async Task<IActionResult> Create([FromBody] ProjectPostDto dto, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            var projectId = await _service.CreateProjectAsync(dto, userId, cancellationToken);
            if (projectId == null)
            {
                return BadRequest();
            }
            return Ok(projectId);
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
        [HttpGet("Get/{id}")]
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
            var projects = await _service.GetProjectByNameAsync(name, cancellationToken);
            if (projects == null || projects.Count == 0)
            {
                return NotFound();
            }
            return Ok(projects);
        }

        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("my/{id}")]
        [Authorize(Policy = nameof(CompanyRole.Employee))]
        public async Task<IActionResult> GetMyById(int id, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            var userProjects = await _service.GetUserProjectsAsync(userId, cancellationToken);
            if (!userProjects.Any(p => p.Id == id))
            {
                return NotFound();
            }
            var project = await _service.GetProjectByIdAsync(id, cancellationToken);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        /// <summary>
        /// Get project by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("my/by-name/{name}")]
        [Authorize(Policy = nameof(CompanyRole.Employee))]
        public async Task<IActionResult> GetMyByName(string name, CancellationToken cancellationToken)
        {
            int userId = GetUserId();
            var userProjects = await _service.GetUserProjectsAsync(userId, cancellationToken);
            var allProjects = await _service.GetProjectByNameAsync(name, cancellationToken);
            var filtered = allProjects.Where(p => userProjects.Any(up => up.Id == p.Id)).ToList();
            if (filtered.Count == 0)
            {
                return NotFound();
            }
            return Ok(filtered);
        }

        /// <summary>
        /// Get projects by company id (admin).
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("by-company/{companyId}")]
        [Authorize(Policy = nameof(CompanyRole.Employee))]
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
        [Authorize(Policy = nameof(CompanyRole.Employee))]
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
        [Authorize(Policy = nameof(CompanyRole.Employee))]
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
        [Authorize(Policy = nameof(CompanyRole.Employee))]
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
        [Authorize(Policy = nameof(CompanyRole.Employee))]
        public async Task<IActionResult> GetProjectUsers(int projectId, CancellationToken cancellationToken)
        {
            var users = await _service.GetProjectUsersAsync(projectId, cancellationToken);
            return Ok(users);
        }

        ///// <summary>
        ///// Add user to project (admin).
        ///// </summary>
        ///// <param name="projectId"></param>
        ///// <param name="userId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpPost("{projectId}/users/{userId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddUserToProject(int projectId, int userId, CancellationToken cancellationToken)
        //{
        //    var result = await _service.AddUserToProjectAsync(projectId, userId, cancellationToken);
        //    if (result == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(new { id = result });
        //}

        ///// <summary>
        ///// Add user to project (owner/manager).
        ///// </summary>
        ///// <param name="projectId"></param>
        ///// <param name="userId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpPost("my/{projectId}/users/{userId}")]
        //[Authorize(Policy = nameof(CompanyRole.Manager))]
        //public async Task<IActionResult> AddUserToProjectOwnerManager(int projectId, int userId, CancellationToken cancellationToken)
        //{
        //    int currentUserId = GetUserId();
        //    var userProjects = await _service.GetUserProjectsAsync(currentUserId, cancellationToken);
            
        //    if (!userProjects.Any(p => p.Id == projectId))
        //    {
        //        return NotFound();
        //    }

        //    var result = await _service.AddUserToProjectAsync(projectId, userId, cancellationToken);
        //    if (result == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(new { id = result });
        //}

        ///// <summary>
        ///// Remove user from project (admin).
        ///// </summary>
        ///// <param name="projectUserId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpDelete("{projectId}/users/{userId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> RemoveUserFromProject(int projectId, int userId, CancellationToken cancellationToken)
        //{
        //    var result = await _service.RemoveUserFromProjectAsync(projectId, userId, cancellationToken);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(new { id = result });
        //}

        ///// <summary>
        ///// Remove user from project (owner/manager).
        ///// </summary>
        ///// <param name="projectId"></param>
        ///// <param name="userId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpDelete("my/{projectId}/users/{userId}")]
        //[Authorize(Policy = nameof(CompanyRole.Manager))]
        //public async Task<IActionResult> RemoveUserFromProjectOwnerManager(int projectId, int userId, CancellationToken cancellationToken)
        //{
        //    int currentUserId = GetUserId();
        //    var userProjects = await _service.GetUserProjectsAsync(currentUserId, cancellationToken);
            
        //    if (!userProjects.Any(p => p.Id == projectId))
        //    {
        //        return NotFound();
        //    }

        //    var result = await _service.RemoveUserFromProjectAsync(projectId, userId, cancellationToken);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(new { id = result });
        //}

        /// <summary>
        /// Update project (admin).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateProjectAsync(id, dto, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Update project (owner/manager).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Update/my/{id}")]
        [Authorize(Policy = nameof(CompanyRole.Manager))]
        public async Task<IActionResult> UpdateOwnerManager(int id, [FromBody] ProjectUpdateDto dto, CancellationToken cancellationToken)
        {
            int currentUserId = GetUserId();
            var userProjects = await _service.GetUserProjectsAsync(currentUserId, cancellationToken);
            
            if (!userProjects.Any(p => p.Id == id))
            {
                return NotFound();
            }

            var result = await _service.UpdateProjectAsync(id, dto, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Delete project by id (admin).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteProjectAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Delete project by id (owner/manager).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("Delete/my/{id}")]
        [Authorize(Policy = nameof(CompanyRole.Manager))]
        public async Task<IActionResult> DeleteOwnerManager(int id, CancellationToken cancellationToken)
        {
            int currentUserId = GetUserId();
            var userProjects = await _service.GetUserProjectsAsync(currentUserId, cancellationToken);
            
            if (!userProjects.Any(p => p.Id == id))
            {
                return NotFound();
            }

            var result = await _service.DeleteProjectAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
