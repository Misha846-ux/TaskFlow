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
        Task<ProjectEntity?> GetProjectByNameAsync(string name, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetAllProjectsAsync(CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsByCompanyIdAsync(int companyId, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsByCompanyIdPaginationAsync(int companyId, int count, int side, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetUserProjectsAsync(int userId, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetUserProjectsPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken);
        Task<ICollection<ProjectUserEntity>> GetProjectUsersAsync(int projectId, CancellationToken cancellationToken);
        Task<ProjectUserEntity?> GetProjectUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<int?> CreateProjectAsync(ProjectEntity project, CancellationToken cancellationToken);
        Task<int?> AddUserToProjectAsync(ProjectUserEntity projectUser, CancellationToken cancellationToken);
        Task<ProjectEntity?> UpdateProjectAsync(ProjectEntity project, CancellationToken cancellationToken);
        Task<ProjectUserEntity?> UpdateProjectUserAsync(ProjectUserEntity projectUser, CancellationToken cancellationToken);
        Task<int?> DeleteProjectByIdAsync(int id, CancellationToken cancellationToken);
        Task<int?> RemoveUserFromProjectAsync(int projectUserId, CancellationToken cancellationToken);
    }
}