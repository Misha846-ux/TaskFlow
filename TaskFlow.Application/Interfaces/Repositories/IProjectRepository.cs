using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<int?> AddProjectAsync(ProjectEntity project, CancellationToken cancellationToken);
        Task<ProjectEntity?> GetProjectByIdAsync(int id, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetAllProjectsAsync(CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetAllProjectsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken);
        Task<ICollection<ProjectEntity>?> GetProjectsByUserIdPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken);
        Task UpdateChangesAsync(CancellationToken cancellationToken);
        Task<int?> DeleteProjectByIdAsync(int id, CancellationToken cancellationToken);

    Task<ICollection<ProjectUserEntity>> GetAllUserProjectsAsync(int projectId, CancellationToken cancellationToken);
        Task<ProjectUserEntity> GetUserInProjectyIdAsync(int id, CancellationToken cancellationToken);
        Task<int?> AddUserToProjectAsync(ProjectUserEntity projectUserEntity, CancellationToken cancellationToken);
        Task<int?> DeleteUserProjectAsync(int id, CancellationToken cancellationToken);
    }
}