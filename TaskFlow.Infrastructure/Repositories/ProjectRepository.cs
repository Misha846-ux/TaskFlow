using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async Task<int?> AddUserToProjectAsync(ProjectUserEntity projectUserEntity, CancellationToken cancellationToken)
        {
            try
            {
                await _context.ProjectUsers.AddAsync(projectUserEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return projectUserEntity.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: AddUserToProjectAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with AddUserToProjectAsync");
            }
        }

        public async Task<int?> DeleteProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _context.Projects.Remove(new ProjectEntity { Id = id });
                await _context.SaveChangesAsync(cancellationToken);
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: DeleteProjectByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with DeleteProjectByIdAsync");
            }
        }

        public async Task<int?> DeleteUserProjectAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _context.ProjectUsers.Remove(new ProjectUserEntity { Id = id });
                await _context.SaveChangesAsync(cancellationToken);
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: DeleteUserProjectAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with DeleteUserProjectAsync");
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Users)
                    .Include(p => p.Company)
                    .Include(p => p.Tasks)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetAllProjectsAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetAllProjectsAsync");
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetAllProjectsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Users)
                    .Include(p => p.Company)
                    .Include(p => p.Tasks)
                    .Where(p => p.Users.Any(u => u.Id == userId))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetAllProjectsByUserIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetAllProjectsByUserIdAsync");
            }
        }

        public async Task<ICollection<ProjectUserEntity>> GetAllUserProjectsAsync(int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.ProjectUsers
                    .Include(u => u.Project)
                    .Include(u => u.User)
                    .Where(u => u.Project.Id == projectId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetAllUserProjectsAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetAllUserProjectsAsync");
            }
        }

        public async Task<ProjectEntity?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Users)
                    .Include(p => p.Tasks)
                    .Include(p => p.Company)
                    .SingleOrDefaultAsync(p =>  p.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetProjectByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetProjectByIdAsync");
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetProjectsByUserIdPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await _context.Projects
                    .Include(p => p.Users)
                    .Include(p => p.Tasks)
                    .Include(p => p.Company)
                    .OrderBy(p => p.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .Where(p => p.Users.Any(p => p.UserId == userId))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetProjectsByUserIdPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetProjectsByUserIdPaginationAsync");
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await this._context.Projects
                    .Include(p => p.Tasks)
                    .Include(p => p.Users)
                    .Include(p => p.Company)
                    .OrderBy(p => p.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetProjectsPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetProjectsPaginationAsync");
            }
        }

        public async Task<ProjectUserEntity> GetUserInProjectyIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.ProjectUsers
                    .Include(u => u.Project)
                    .Include(u => u.User)
                    .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: GetUserInProjectyIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with GetUserInProjectyIdAsync");
            }
        }

        public async Task<ProjectEntity> UpdateProjectAsync(ProjectEntity newProject, CancellationToken cancellationToken)
        {
            try
            {
                ProjectEntity project = await GetProjectByIdAsync(newProject.Id, cancellationToken);
                if (project == null)
                {
                    throw new Exception();
                }
                PropertyInfo[] properties = typeof(ProjectEntity).GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    if(prop.GetValue(newProject) != null)
                    {
                        prop.SetValue(project, newProject);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                return project;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: UpdateProjectAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with UpdateProjectAsync");
            }
        }

        public async Task<ProjectUserEntity> UpdateProjectUserAsync(ProjectUserEntity newProjectUser, CancellationToken cancellationToken)
        {
            try
            {
                ProjectUserEntity projectUser = await GetUserInProjectyIdAsync(newProjectUser.Id, cancellationToken);
                if (projectUser == null)
                {
                    throw new Exception();
                }
                PropertyInfo[] properties = typeof(ProjectUserEntity).GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    if(prop.GetValue(newProjectUser) != null)
                    {
                        prop.SetValue(projectUser, newProjectUser);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                return newProjectUser;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Project Repository: UpdateProjectUserAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Project Repository: Problem with UpdateProjectUserAsync");
            }
        }
    }
}
