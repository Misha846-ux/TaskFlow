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
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cacheService;

        public ProjectService(IProjectRepository repository, IMapper mapper, ICachingService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<int?> CreateProjectAsync(CreateProjectDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ProjectEntity>(dto);
            await _cacheService.RemoveAsync("Projects");
            await _cacheService.RemoveAsync("Projects:admin");

            return await _repository.AddProjectAsync(entity, cancellationToken);
        }

        public async Task<ProjectDetailsDto?> GetProjectByIdAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ProjectDetailsDto>($"Projects:user:id:{userId}:{id}");
            if (cache == null)
            {
                var project = await _repository.GetProjectByIdAsync(id, cancellationToken);

                if (project == null)
                    return null;

                var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

                if (!userProjects.Any(x => x.Id == id))
                    return null;

                cache = _mapper.Map<ProjectDetailsDto>(project);
                await _cacheService.SetAsync($"Projects:user:id:{userId}:{id}", cache, null);
            }
            return cache;
        }

        public async Task<ProjectDetailsDto?> GetProjectByIdAdminAsync(int id, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ProjectDetailsDto>($"Projects:admin:id:{id}");
            if (cache == null)
            {
                var project = await _repository.GetProjectByIdAsync(id, cancellationToken);

                if (project == null)
                    return null;

                cache = _mapper.Map<ProjectDetailsDto>(project);
                await _cacheService.SetAsync($"Projects:admin:id:{id}", cache, null);
            }
            return cache;
        }

        public async Task<PagedResponseDto<ProjectListItemDto>> GetProjectsAsync(ProjectFilterDto filter, int userId, CancellationToken cancellationToken)
        {
            var search = filter.Search?.ToLower() ?? "all";
            var cache = await _cacheService.GetAsync<PagedResponseDto<ProjectListItemDto>>($"Projects:user:{userId}:list:{search}:{filter.Page}:{filter.PageSize}");
            if (cache == null)
            {
                var projects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

                var query = projects.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.Search))
                    query = query.Where(x => x.Title.ToLower().Contains(search));

                var totalCount = query.Count();

                var items = query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                cache = new PagedResponseDto<ProjectListItemDto>
                {
                    Items = _mapper.Map<List<ProjectListItemDto>>(items),
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
                };
                await _cacheService.SetAsync($"Projects:user:{userId}:list:{search}:{filter.Page}:{filter.PageSize}", cache, null);
            }
            return cache;
        }

        public async Task<PagedResponseDto<ProjectListItemDto>> GetProjectsAdminAsync(ProjectFilterDto filter, CancellationToken cancellationToken)
        {
            var search = filter.Search?.ToLower() ?? "all";
            var cache = await _cacheService.GetAsync<PagedResponseDto<ProjectListItemDto>>($"Projects:admin:list:{search}:{filter.Page}:{filter.PageSize}");
            if (cache == null)
            {
                var projects = await _repository.GetAllProjectsAsync(cancellationToken);

                var query = projects.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.Search))
                    query = query.Where(x => x.Title.ToLower().Contains(search));

                var totalCount = query.Count();

                var items = query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                cache = new PagedResponseDto<ProjectListItemDto>
                {
                    Items = _mapper.Map<List<ProjectListItemDto>>(items),
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
                };
                await _cacheService.SetAsync($"Projects:admin:list:{search}:{filter.Page}:{filter.PageSize}", cache, null);
            }
            return cache;
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
            var cache = await _cacheService.GetAsync<ICollection<ProjectUserListItemDto>>($"Projects:user:{projectId}:{userId}");
            if (cache == null)
            {
                var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

                if (!userProjects.Any(x => x.Id == projectId))
                    return new List<ProjectUserListItemDto>();

                var users = await _repository.GetAllProjectUsersAsync(projectId, cancellationToken);

                cache = _mapper.Map<ICollection<ProjectUserListItemDto>>(users);
                await _cacheService.SetAsync($"Projects:user:{projectId}:{userId}", cache, null);
            }
            return cache;
        }

        public async Task<ICollection<ProjectUserListItemDto>> GetProjectUsersAdminAsync(int projectId, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectUserListItemDto>>($"Projects:admin:{projectId}");
            if (cache == null)
            {
                var users = await _repository.GetAllProjectUsersAsync(projectId, cancellationToken);

                cache = _mapper.Map<ICollection<ProjectUserListItemDto>>(users);
                await _cacheService.SetAsync($"Projects:admin:{projectId}", cache, null);
            }
            return cache;
        }

        public async Task<int?> AddUserToProjectAsync(int projectId, AddUserToProjectDto dto, CancellationToken cancellationToken)
        {
            var entity = new ProjectUserEntity
            {
                ProjectId = projectId,
                UserId = dto.UserId,
                ProjectRole = dto.ProjectRole
            };
            await _cacheService.RemoveAsync($"Projects:admin:users:{projectId}");
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

            ProjectEntity project = _mapper.Map<ProjectEntity>(userProjects);

            await _repository.UpdateProjectAsync(project, cancellationToken);
            await _cacheService.RemoveAsync($"Projects:user:{userId}:id:{id}");
            await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
        }

        public async Task UpdateProjectAdminAsync(int id, UpdateProjectDto dto, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<ProjectEntity>(dto);
            project.Id = id;
            await _repository.UpdateProjectAsync(project, cancellationToken);
            await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
        }

        public async Task<int?> DeleteProjectAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var userProjects = await _repository.GetAllProjectsByUserIdAsync(userId, cancellationToken);

            if (!userProjects.Any(x => x.Id == id))
                return null;
            await _cacheService.RemoveAsync($"Projects:user:{userId}:id:{id}");
            await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
            return await _repository.DeleteProjectByIdAsync(id, cancellationToken);
        }

        public async Task<int?> DeleteProjectAdminAsync(int id, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
            return await _repository.DeleteProjectByIdAsync(id, cancellationToken);
        }
    }
}