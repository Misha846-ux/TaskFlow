using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskFlowDbContext _context;
        public ProjectRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<int?> AddProjectAsync(ProjectEntity project, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Projects.AddAsync(project, cancellationToken);
                //Adding company owner to new project
                ICollection<CompanyUserEntity> owner = await _context.CompanyUsers
                    .Where(c => c.CompanyId == project.CompanyId)
                    .Where(c => c.CompanyRole == Domain.Enums.CompanyRole.Owner)
                    .ToListAsync(cancellationToken);
                foreach (var user in owner)
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUserEntity
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        ProjectRole = Domain.Enums.ProjectRole.ProjectManager
                    },cancellationToken);
                }
                //Adding company managers to new project
                ICollection<CompanyUserEntity> managers = await _context.CompanyUsers
                    .Where(c => c.CompanyId == project.CompanyId)
                    .Where(c => c.CompanyRole == Domain.Enums.CompanyRole.Manager)
                    .ToListAsync(cancellationToken);
                foreach (var user in managers)
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUserEntity
                    {
                        ProjectId = project.Id,
                        UserId = user.Id,
                        ProjectRole = Domain.Enums.ProjectRole.ProjectManager
                    }, cancellationToken);
                }
                return project.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: AddProjectAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with AddProjectAsync");
            }
        }

        public Task<int?> AddUserToProjectAsync(ProjectUserEntity projectUserEntity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int?> DeleteProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int?> DeleteUserProjectAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectEntity>?> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectEntity>?> GetAllProjectsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectUserEntity>> GetAllProjectUsersAsync(int projectId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectUserEntity>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectEntity?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectEntity>?> GetProjectsByUserIdPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectUserEntity> GetUserInProjectyIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
