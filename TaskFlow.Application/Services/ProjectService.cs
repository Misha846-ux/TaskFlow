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
using TaskFlow.Application.Exceptions;
using System.ComponentModel.Design;

namespace TaskFlow.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cacheService;
        private readonly IChangeService _changeService;

        public ProjectService(IProjectRepository projectRepository, ICompanyRepository companyRepository, IMapper mapper, ICachingService cacheService, IChangeService changeService)
        {
            _projectRepository = projectRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _changeService = changeService;
        }

        public async Task<int?> CreateProjectAsync(ProjectPostDto dto, int currentUserId, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                throw new BadRequestException("Project data is required");
            }
            var company = await _companyRepository.GetCompanyByIdAsync(dto.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new NotFoundException($"Company with id {dto.CompanyId} not found");
            }
            var companyUser = company.Users?.FirstOrDefault(cu => cu.UserId == currentUserId);
            if (companyUser == null)
            {
                throw new ForbiddenException("User does not belong to the company");
            }
            if (companyUser.CompanyRole == CompanyRole.Employee)
            {
                throw new ForbiddenException("Employee cannot create a project");
            }
            var entity = _mapper.Map<ProjectEntity>(dto);
            var createdId = await _projectRepository.CreateProjectAsync(entity, cancellationToken);
            if (createdId != null)
            {
                await _cacheService.RemoveAsync("Projects:admin");
                await _cacheService.RemoveAsync($"Projects:admin:company:{dto.CompanyId}");
                await _cacheService.RemoveAsync($"Projects:admin:pagination");
                await _cacheService.RemoveAsync($"Projects:admin:company:{dto.CompanyId}:pagination");
                await _cacheService.RemoveAsync($"Projects:user:pagination");
                var userIds = company.Users?.Where(cu => cu.UserId.HasValue).Select(cu => cu.UserId.Value).ToList() ?? new List<int>();
                await _changeService.CreateChangeAsync(ChangeTableType.Projects, createdId.Value, ChangeType.Created, userIds, cancellationToken);
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
                throw new NotFoundException($"Project with id {id} not found");
            }
            var dto = _mapper.Map<ProjectGetDto>(entity);
            await _cacheService.SetAsync($"Projects:admin:id:{id}", dto, null);
            return dto;
        }

        public async Task<ICollection<ProjectGetDto>> GetProjectByNameAsync(string name, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:admin:name:{name}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetProjectsByNameAsync(name, cancellationToken);
            if (entities == null || !entities.Any())
            {
                return new List<ProjectGetDto>();
            }
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities);
            await _cacheService.SetAsync($"Projects:admin:name:{name}", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>("Projects:admin");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetAllProjectsAsync(cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync("Projects:admin", dtos, null);
            return dtos;
        }

        public async Task<ICollection<ProjectGetDto>> GetProjectsPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:admin:pagination:{count}:{side}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetProjectsPaginationAsync(count, side, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:admin:pagination:{count}:{side}", dtos, null);
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
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:admin:company:{companyId}:pagination:{count}:{side}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetProjectsByCompanyIdPaginationAsync(companyId, count, side, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:admin:company:{companyId}:pagination:{count}:{side}", dtos, null);
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
            var cache = await _cacheService.GetAsync<ICollection<ProjectGetDto>>($"Projects:user:{userId}:pagination:{count}:{side}");
            if (cache != null)
            {
                return cache;
            }
            var entities = await _projectRepository.GetUserProjectsPaginationAsync(userId, count, side, cancellationToken);
            var dtos = _mapper.Map<ICollection<ProjectGetDto>>(entities ?? new List<ProjectEntity>());
            await _cacheService.SetAsync($"Projects:user:{userId}:pagination:{count}:{side}", dtos, null);
            return dtos;
        }

        public async Task<int?> UpdateProjectAsync(int id, ProjectUpdateDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                throw new BadRequestException("Project data is required");
            }
            var existingProject = await _projectRepository.GetProjectByIdAsync(id, cancellationToken);
            if (existingProject == null)
            {
                throw new NotFoundException($"Project with id {id} not found");
            }
            existingProject.Title = dto.Title ?? existingProject.Title;
            var result = await _projectRepository.UpdateProjectAsync(existingProject, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
                await _cacheService.RemoveAsync("Projects:admin");
                await _cacheService.RemoveAsync($"Projects:admin:company:{existingProject.CompanyId}");
                await _cacheService.RemoveAsync($"Projects:admin:pagination");
                await _cacheService.RemoveAsync($"Projects:admin:company:{existingProject.CompanyId}:pagination");
                await _cacheService.RemoveAsync($"Projects:user:pagination");
                var userIds = existingProject.Users?.Select(u => u.Id).ToList() ?? new List<int>();
                await _changeService.CreateChangeAsync(ChangeTableType.Projects, id, ChangeType.Updated, userIds, cancellationToken);
            }
            return result?.Id;
        }

        public async Task<int?> AddUserToProjectAsync(int projectId, int userId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId, cancellationToken);
            if (project == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found");
            }
            if (project.Users?.Any(u => u.Id == userId) == true)
            {
                throw new BadRequestException($"User with id {userId} is already added to project {projectId}");
            }
            var result = await _projectRepository.AddUserToProjectAsync(userId, projectId, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:user:{userId}:list");
                await _cacheService.RemoveAsync($"Projects:{projectId}:users");
                await _cacheService.RemoveAsync("Projects:admin");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}");
                await _cacheService.RemoveAsync($"Projects:admin:pagination");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}:pagination");
                await _cacheService.RemoveAsync($"Projects:user:pagination");

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
            var dtos = _mapper.Map<ICollection<UserGetDto>>(projectUsers ?? new List<UserEntity>());
            await _cacheService.SetAsync($"Projects:{projectId}:users", dtos, null);
            return dtos;
        }

        public async Task<int?> RemoveUserFromProjectAsync(int projectId, int userId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId, cancellationToken);
            if (project == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found");
            }
            var isInProject = project.Users?.Any(u => u.Id == userId) ?? false;
            if (!isInProject)
            {
                throw new NotFoundException($"User with id {userId} not found in project {projectId}");
            }
            var result = await _projectRepository.RemoveUserFromProjectAsync(userId, projectId, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:user:{userId}:list");
                await _cacheService.RemoveAsync($"Projects:{projectId}:users");
                await _cacheService.RemoveAsync("Projects:admin");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}");
                await _cacheService.RemoveAsync($"Projects:admin:pagination");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}:pagination");
                await _cacheService.RemoveAsync($"Projects:user:pagination");
            }
            return result;
        }

        public async Task<int?> DeleteProjectAsync(int id, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id, cancellationToken);
            if (project == null)
            {
                throw new NotFoundException($"Project with id {id} not found");
            }
            var result = await _projectRepository.DeleteProjectByIdAsync(id, cancellationToken);
            if (result != null)
            {
                await _cacheService.RemoveAsync($"Projects:admin:id:{id}");
                await _cacheService.RemoveAsync("Projects:admin");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}");
                await _cacheService.RemoveAsync($"Projects:admin:pagination");
                await _cacheService.RemoveAsync($"Projects:admin:company:{project.CompanyId}:pagination");
                await _cacheService.RemoveAsync($"Projects:user:pagination");
                var userIds = project.Users?.Select(u => u.Id).ToList() ?? new List<int>();
                await _changeService.CreateChangeAsync(ChangeTableType.Projects, id, ChangeType.Deleted, userIds, cancellationToken);
            }
            return result;
        }
    }
}
