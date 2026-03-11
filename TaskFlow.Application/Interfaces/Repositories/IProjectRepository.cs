using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<int?> AddProject(ProjectEntity project);
    Task<ProjectEntity?> GetProjectById(int id);
    Task<ICollection<ProjectEntity>?> GetAllProjects();
    Task<ICollection<ProjectEntity>?> GetAllProjectsByUserId(int userId);
    Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side);
    Task<ICollection<ProjectEntity>?> GetProjectsByUserIdPaginationAsync(int userId, int count, int side);
    Task<int?> UpdateProject();
    Task<int?> DeleteProjectById(int id);

    Task<ICollection<ProjectUserEntity>> GetAllUsersAsync();
    Task<ICollection<ProjectUserEntity>> GetAllProjectUsersAsync(int projectId);
    Task<ProjectUserEntity> GetUserInProjectyIdAsync(int id);
    Task<int?> AddUserToProjectAsync(ProjectUserEntity projectUserEntity);
    Task<int?> DeleteUserProjectAsync(int id);
}
