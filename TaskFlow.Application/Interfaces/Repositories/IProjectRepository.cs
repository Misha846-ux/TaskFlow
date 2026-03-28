using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectEntity?> GetProjectByIdAsync(int id, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsByNameAsync(string name, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetAllProjectsAsync(CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetCompanyProjectsByNameAsync(string name, int companyId, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsByCompanyIdAsync(int companyId, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsByCompanyIdPaginationAsync(int companyId, int count, int side, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetUserProjectsAsync(int userId, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetUserProjectsPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken);
        Task<ICollection<UserEntity>> GetProjectUsersAsync(int projectId, CancellationToken cancellationToken);
        Task<int?> CreateProjectAsync(ProjectEntity project, CancellationToken cancellationToken);
        Task<int?> AddUserToProjectAsync(int userId, int projectId, CancellationToken cancellationToken);
        /// <summary>
        /// Внутрь поступает липовый объект с id в котором указаны поля которые должны быть изменены а
        /// поля которые не должны быть измененны остаются null либо string.empty
        /// </summary>
        /// <param name="project"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ProjectEntity?> UpdateProjectAsync(ProjectEntity project, CancellationToken cancellationToken);
        Task<int?> DeleteProjectByIdAsync(int id, CancellationToken cancellationToken);
        Task<int?> RemoveUserFromProjectAsync(int userId, int projectId, CancellationToken cancellationToken);
    }
}