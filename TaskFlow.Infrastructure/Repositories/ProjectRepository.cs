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

        public async Task<ProjectEntity?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Project by Id", ex);
            }
        }

        public async Task<ProjectEntity?> GetProjectByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .FirstOrDefaultAsync(p => p.Title == name, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Project by Name", ex);
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get All Projects", ex);
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if (side < 1)
                {
                    side = 1;
                }
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .OrderBy(p => p.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Projects", ex);
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetProjectsByCompanyIdAsync(int companyId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Projects Company", ex);
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetProjectsByCompanyIdPaginationAsync(int companyId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if (side < 1)
                {
                    side = 1;
                }
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .Where(p => p.CompanyId == companyId)
                    .OrderBy(p => p.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Projects Company", ex);
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetUserProjectsAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .Where(p => p.Users.Any(u => u.UserId == userId))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get User Projects", ex);
            }
        }

        public async Task<ICollection<ProjectEntity>?> GetUserProjectsPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if (side < 1)
                {
                    side = 1;
                }
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .ThenInclude(pu => pu.User)
                    .Include(p => p.Tasks)
                    .Where(p => p.Users.Any(u => u.UserId == userId))
                    .OrderBy(p => p.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get User Projects", ex);
            }
        }

        public async Task<ICollection<ProjectUserEntity>> GetProjectUsersAsync(int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.ProjectUsers
                    .Include(pu => pu.User)
                    .Where(pu => pu.ProjectId == projectId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Project Users", ex);
            }
        }

        public async Task<ProjectUserEntity?> GetProjectUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.ProjectUsers
                    .Include(pu => pu.Project)
                    .Include(pu => pu.User)
                    .FirstOrDefaultAsync(pu => pu.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Project by Id", ex);
            }
        }

        public async Task<int?> CreateProjectAsync(ProjectEntity project, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Projects.AddAsync(project, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return project.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Create Project", ex);
            }
        }

        public async Task<int?> AddUserToProjectAsync(ProjectUserEntity projectUser, CancellationToken cancellationToken)
        {
            try
            {
                await _context.ProjectUsers.AddAsync(projectUser, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return projectUser.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Add User to Project", ex);
            }
        }

        public async Task<ProjectEntity?> UpdateProjectAsync(ProjectEntity project, CancellationToken cancellationToken)
        {
            try
            {
                _context.Projects.Update(project);
                await _context.SaveChangesAsync(cancellationToken);
                return project;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Update Project", ex);
            }
        }

        public async Task<ProjectUserEntity?> UpdateProjectUserAsync(ProjectUserEntity projectUser, CancellationToken cancellationToken)
        {
            try
            {
                _context.ProjectUsers.Update(projectUser);
                await _context.SaveChangesAsync(cancellationToken);
                return projectUser;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Update Project", ex);
            }
        }

        public async Task<int?> DeleteProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var affectedRows = await _context.Projects
                    .Where(p => p.Id == id)
                    .ExecuteDeleteAsync(cancellationToken);
                if (affectedRows == 0)
                {
                    return null;
                }
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Delete Project", ex);
            }
        }

        public async Task<int?> RemoveUserFromProjectAsync(int projectUserId, CancellationToken cancellationToken)
        {
            try
            {
                var affectedRows = await _context.ProjectUsers
                    .Where(pu => pu.Id == projectUserId)
                    .ExecuteDeleteAsync(cancellationToken);
                if (affectedRows == 0)
                {
                    return null;
                }
                return projectUserId;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Remove User", ex);
            }
        }
    }
}

