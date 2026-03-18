using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int?> CreateProjectAsync(ProjectPostDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a project by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ProjectGetDto?> GetProjectByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a project by name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ProjectGetDto?> GetProjectByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ProjectGetDto>> GetAllProjectsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns projects portion.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ProjectGetDto>> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken);

        /// <summary>
        /// Returns projects by company id.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ProjectGetDto>> GetProjectsByCompanyIdAsync(int companyId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns projects by company id portion.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ProjectGetDto>> GetProjectsByCompanyIdPaginationAsync(int companyId, int count, int side, CancellationToken cancellationToken);

        /// <summary>
        /// Returns projects by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ProjectGetDto>> GetUserProjectsAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns projects by user Id portion.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ProjectGetDto>> GetUserProjectsPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a project.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int?> UpdateProjectAsync(int id, ProjectUpdateDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a user to a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int?> AddUserToProjectAsync(int projectId, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns users of a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<UserGetDto>> GetProjectUsersAsync(int projectId, CancellationToken cancellationToken);

        /// <summary>
        /// Removes a user from a project.
        /// </summary>
        /// <param name="projectUserId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int?> RemoveUserFromProjectAsync(int projectUserId, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a project by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int?> DeleteProjectAsync(int id, CancellationToken cancellationToken);
    }
}
