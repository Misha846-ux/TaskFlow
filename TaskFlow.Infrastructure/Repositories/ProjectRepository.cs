using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;
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
                    .Include(p => p.Tasks)
                    .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
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

        public async Task<ICollection<ProjectEntity>?> GetProjectsByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .Include(p => p.Tasks)
                    .Where(p => p.Title.Contains(name))
                    .ToListAsync(cancellationToken);
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
                    .Include(p => p.Tasks)
                    .Where(p => p.Users.Any(u => u.Id == userId))
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
                    .Include(p => p.Tasks)
                    .Where(p => p.Users.Any(u => u.Id == userId))
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

        public async Task<ICollection<UserEntity>> GetProjectUsersAsync(int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Changes)
                    .Where(u => u.Projects.Any(p => p.Id == projectId))
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

        public async Task<int?> CreateProjectAsync(ProjectEntity project, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Projects.AddAsync(project, cancellationToken);
                //Adding company owner to new project
                ICollection<CompanyUserEntity> owner = await _context.CompanyUsers
                    .Include(pu => pu.User)
                    .Where(c => c.CompanyId == project.CompanyId)
                    .Where(c => c.CompanyRole == Domain.Enums.CompanyRole.Owner)
                    .ToListAsync(cancellationToken);
                foreach (var user in owner)
                {
                    project.Users.Add(user.User);
                }
                //Adding company managers to new project
                ICollection<CompanyUserEntity> managers = await _context.CompanyUsers
                    .Include(pu => pu.User)
                    .Where(c => c.CompanyId == project.CompanyId)
                    .Where(c => c.CompanyRole == Domain.Enums.CompanyRole.Manager)
                    .ToListAsync(cancellationToken);
                foreach (var user in managers)
                {
                    project.Users.Add(user.User);
                }
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

        public async Task<int?> AddUserToProjectAsync(int userId, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                ProjectEntity? project = await _context.Projects
                    .Include(p => p.Users)
                    .SingleOrDefaultAsync(p => p.Id == projectId, cancellationToken);
                if (project == null)
                {
                    throw new Exception("Project = null");
                }
                UserEntity? user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
                if (user == null)
                {
                    throw new Exception("User = null");
                }
                project.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
                return user.Id;
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

        public async Task<ProjectEntity?> UpdateProjectAsync(ProjectEntity newProject, CancellationToken cancellationToken)
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
                    var value = prop.GetValue(newProject);
                    if (value != null && value != string.Empty)
                    {
                        prop.SetValue(project, value);
                    }
                }
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
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Delete Project", ex);
            }
        }

        public async Task<int?> RemoveUserFromProjectAsync(int userId, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                ProjectEntity? project = await _context.Projects
                    .Include(p => p.Users)
                    .SingleOrDefaultAsync(p => p.Id == projectId, cancellationToken);
                if (project == null)
                {
                    throw new Exception("Project = null");
                }
                UserEntity? user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
                if (user == null)
                {
                    throw new Exception("User = null");
                }
                project.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return projectId;
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

        public async Task<ICollection<ProjectEntity>?> GetCompanyProjectsByNameAsync(string name, int companyId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.Company)
                    .Include(p => p.Users)
                    .Include(p => p.Tasks)
                    .Where(p => p.Title.Contains(name))
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync(cancellationToken);
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

