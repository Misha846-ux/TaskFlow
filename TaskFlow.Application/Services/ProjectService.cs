using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using AutoMapper;
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cacheService;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper, ICachingService cacheService)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<int?> CreateProjectAsync(ProjectPostDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            var entity = _mapper.Map<ProjectEntity>(dto);
            var createdId = await _projectRepository.CreateProjectAsync(entity, cancellationToken);
            if (createdId != null)
            {
                await _cacheService.RemoveAsync("Projects:admin:list");
                await _cacheService.RemoveAsync($"Projects:admin:company:{dto.CompanyId}");
            }
            return createdId;
        }

        public async Task<ProjectGetDto?> GetProjectByIdAsync(int id, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ProjectGetDto>($"Projects:admin:id:{id}");
            if (cache != null)
            {
                return cache;
            }
            var entity = await _projectRepository.GetProjectByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                return null;
            }
            var dto = _mapper.Map<ProjectGetDto>(entity);
            await _cacheService.SetAsync($"Projects:admin:id:{id}", dto, null);
            return dto;
        }

        public async Task<ProjectGetDto?> GetProjectByNameAsync(string name, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ProjectGetDto>($"Projects:admin:name:{name}");
            if (cache != null)
            {
                return cache;
            }
            var entity = await _projectRepository.GetProjectByNameAsync(name, cancellationToken);
            if (entity == null)
            {
                return null;
            }
            var dto = _mapper.Map<ProjectGetDto>(entity);
            await _cacheService.SetAsync($"Projects:admin:name:{name}", dto, null);
            return dto;
        }

        public async Task<ICollection<ProjectGetDto>> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>("Projects:admin:list");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetAllProjectsAsync(cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync("Projects:admin:list", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:admin:list:{count}:{side}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetProjectsPaginationAsync(count, side, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:admin:list:{count}:{side}", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetProjectsByCompanyIdAsync(int companyId, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:admin:company:{companyId}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetProjectsByCompanyIdAsync(companyId, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:admin:company:{companyId}", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetProjectsByCompanyIdPaginationAsync(int companyId, int count, int side, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:admin:company:{companyId}:{count}:{side}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetProjectsByCompanyIdPaginationAsync(companyId, count, side, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:admin:company:{companyId}:{count}:{side}", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetUserProjectsAsync(int userId, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:user:{userId}:list");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetUserProjectsAsync(userId, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:user:{userId}:list", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetUserProjectsPaginationAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:user:{userId}:list:{count}:{side}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetUserProjectsPaginationAsync(userId, count, side, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:user:{userId}:list:{count}:{side}", dtos, null);
            return dtos;
        }

        public async Task<int?> UpdateProjectAsync(int id, ProjectUpdateDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            var project = await _projectRepository.GetProjectByIdAsync(id, cancellationToken);
            if (project == null)
            {
                return null;
            }
            var entity = _mapper.Map<ProjectEntity>(dto);
            entity.Id = id;
            var result = await _projectRepository.UpdateProjectAsync(entity, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
                await _cacheService.RemoveAsync("Projects:admin:list");
            }
            return result?.Id;
        }

        public async Task<int?> AddUserToProjectAsync(int projectId, int userId, CancellationToken cancellationToken)
        {
            var projectUser = new ProjectUserEntity
            {
                ProjectId = projectId,
                UserId = userId
            };
            var result = await _projectRepository.AddUserToProjectAsync(projectUser, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:user:{userId}:list");
                await _cacheService.RemoveAsync($"Projects:{projectId}:users");
            }
            return result;
        }

        public async Task<ICollection<UserGetDto>> GetProjectUsersAsync(int projectId, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<UserGetDto>>($"Projects:{projectId}:users");
            if (cache != null)
            {
                return cache;
            }
            var projectUsers = await _projectRepository.GetProjectUsersAsync(projectId, cancellationToken);
            var dtos = _mapper.Map<ICollection<UserGetDto>>(projectUsers?.Select(pu => pu.User) ?? new List<UserEntity>());
            await _cacheService.SetAsync($"Projects:{projectId}:users", dtos, null);
            return dtos;
        }

        public async Task<int?> RemoveUserFromProjectAsync(int projectUserId, CancellationToken cancellationToken)
        {
            var projectUser = await _projectRepository.GetProjectUserByIdAsync(projectUserId, cancellationToken);
            if (projectUser == null)
            {
                return null;
            }
            var result = await _projectRepository.RemoveUserFromProjectAsync(projectUserId, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:user:{projectUser.UserId}:list");
                await _cacheService.RemoveAsync($"Projects:{projectUser.ProjectId}:users");
            }
            return result;
        }

        public async Task<int?> DeleteProjectAsync(int id, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id, cancellationToken);
            if (project == null)
            {
                return null;
            }
            var result = await _projectRepository.DeleteProjectByIdAsync(id, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
                await _cacheService.RemoveAsync("Projects:admin:list");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}");
            }
            return result;
        }
    }
}
