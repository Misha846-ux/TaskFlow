using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs.CommonDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.CreateProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.DetailsProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs;
using TaskFlow.Application.DTOs.ProjectDTOs.UpdateProjectDTOs;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using AutoMapper;

namespace TaskFlow.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int?> CreateProjectAsync(CreateProjectDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ProjectEntity>(dto);

            return await _repository.AddProjectAsync(entity, cancellationToken);
        }

        public async Task<ProjectDetailsDto?> GetProjectByIdAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var project = await _repository.GetProjectByIdAsync(id, cancellationToken);

            if (project == null)
                return null;

            var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            if (!userProjects.Any(x => x.Id == id))
                return null;

            return _mapper.Map<ProjectDetailsDto>(project);
        }

        public async Task<ProjectDetailsDto?> GetProjectByIdAdminAsync(int id, CancellationToken cancellationToken)
        {
            var project = await _repository.GetProjectByIdAsync(id, cancellationToken);

            if (project == null)
                return null;

            return _mapper.Map<ProjectDetailsDto>(project);
        }

        public async Task<PagedResponseDto<ProjectListItemDto>> GetProjectsAsync(ProjectFilterDto filter, int userId, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            var query = projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Title.ToLower().Contains(filter.Search.ToLower()));

            var totalCount = query.Count();

            var items = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResponseDto<ProjectListItemDto>
            {
                Items = _mapper.Map<List<ProjectListItemDto>>(items),
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }

        public async Task<PagedResponseDto<ProjectListItemDto>> GetProjectsAdminAsync(ProjectFilterDto filter, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAllProjectsAsync(cancellationToken);

            var query = projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Title.ToLower().Contains(filter.Search.ToLower()));

            var totalCount = query.Count();

            var items = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResponseDto<ProjectListItemDto>
            {
                Items = _mapper.Map<List<ProjectListItemDto>>(items),
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }

        public async Task<PagedResponseDto<ProjectListItemDto>> GetProjectsByNameAsync(string name, int userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            var query = projects
                .Where(x => x.Title.ToLower().Contains(name.ToLower()))
                .AsQueryable();

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponseDto<ProjectListItemDto>
            {
                Items = _mapper.Map<List<ProjectListItemDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<PagedResponseDto<ProjectListItemDto>> GetProjectsByNameAdminAsync(string name, int page, int pageSize, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAllProjectsAsync(cancellationToken);

            var query = projects
                .Where(x => x.Title.ToLower().Contains(name.ToLower()))
                .AsQueryable();

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponseDto<ProjectListItemDto>
            {
                Items = _mapper.Map<List<ProjectListItemDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<ICollection<ProjectUserListItemDto>> GetProjectUsersAsync(int projectId, int userId, CancellationToken cancellationToken)
        {
            var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            if (!userProjects.Any(x => x.Id == projectId))
                return new List<ProjectUserListItemDto>();

            var users = await _repository.GetAllUserProjectsAsync(projectId, cancellationToken);

            return _mapper.Map<ICollection<ProjectUserListItemDto>>(users);
        }

        public async Task<ICollection<ProjectUserListItemDto>> GetProjectUsersAdminAsync(int projectId, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllUserProjectsAsync(projectId, cancellationToken);

            return _mapper.Map<ICollection<ProjectUserListItemDto>>(users);
        }

        public async Task<int?> AddUserToProjectAsync(int projectId, AddUserToProjectDto dto, CancellationToken cancellationToken)
        {
            var entity = new ProjectUserEntity
            {
                ProjectId = projectId,
                UserId = dto.UserId,
                ProjectRole = dto.ProjectRole
            };

            return await _repository.AddUserToProjectAsync(entity, cancellationToken);
        }

        public async Task<int?> RemoveUserFromProjectAsync(int projectUserId, CancellationToken cancellationToken)
        {
            return await _repository.DeleteUserProjectAsync(projectUserId, cancellationToken);
        }

        public async Task UpdateProjectAsync(int id, UpdateProjectDto dto, int userId, CancellationToken cancellationToken)
        {
            var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            if (!userProjects.Any(x => x.Id == id))
                return;

            var project = await _repository.GetProjectByIdAsync(id, cancellationToken);

            project.Title = dto.Title;

            await _repository.UpdateChangesAsync(cancellationToken);
        }

        public async Task UpdateProjectAdminAsync(int id, UpdateProjectDto dto, CancellationToken cancellationToken)
        {
            var project = await _repository.GetProjectByIdAsync(id, cancellationToken);

            project.Title = dto.Title;

            await _repository.UpdateChangesAsync(cancellationToken);
        }

        public async Task<int?> DeleteProjectAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            if (!userProjects.Any(x => x.Id == id))
                return null;
             
            return await _repository.DeleteProjectByIdAsync(id, cancellationToken);
        }

        public async Task<int?> DeleteProjectAdminAsync(int id, CancellationToken cancellationToken)
        {
            return await _repository.DeleteProjectByIdAsync(id, cancellationToken);
        }
    }
}