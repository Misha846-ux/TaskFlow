using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskFlow.Application.DTOs.TaskDto;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskFlow.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cacheService;
        private readonly IChangeService _changeService;
        public TaskService(ITaskRepository repository, IProjectRepository projectRepository, IMapper mapper, ICachingService cacheService, IChangeService changeService)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _changeService = changeService;
        }
        //Method for Task creating
        public async Task<int?> CreateTaskAsync(TaskPostDto dto, CancellationToken cancellationToken)
        {
            try
            {   
                var task = _mapper.Map<TaskEntity>(dto);
                await _cacheService.RemoveAsync($"Tasks:status:{task.Status}:{task.ProjectId}");
                await _cacheService.RemoveAsync("Tasks");
                await _cacheService.RemoveAsync($"Tasks:deadline:{task.ProjectId}:{task.DeadLine:yyyyMMdd}");
                await _cacheService.RemoveAsync($"Tasks:name:{task.ProjectId}:{task.TaskName.ToLower()}");
                await _cacheService.RemoveAsync("Tasks:pagination");
                await _cacheService.RemoveAsync($"Tasks:pagination:{task.ProjectId}");
                await _cacheService.RemoveAsync($"Tasks:name::pagination:{task.ProjectId}:{task.TaskName.ToLower()}");

                var createdId = await _repository.AddTaskAsync(task, cancellationToken);
                if (createdId != null)
                {
                    var project = await _projectRepository.GetProjectByIdAsync(task.ProjectId.Value, cancellationToken);
                    var userIds = project?.Users?.Select(u => u.Id).ToList() ?? new List<int>();
                    await _changeService.CreateChangeAsync(ChangeTableType.Tasks, createdId.Value, ChangeType.Created, userIds, cancellationToken);
                }
                return createdId;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: CreateTasksAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with CreateTaskAsync");
            }
        }
        //Method for Task deleting by id
        public async Task<bool> DeleteTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {

                var task = await _repository.GetTaskByIdAsync(id, cancellationToken);

                if (task == null)
                    return false;

                await _cacheService.RemoveAsync("Tasks");
                await _cacheService.RemoveAsync($"Tasks:id:{id}");
                await _cacheService.RemoveAsync($"Tasks:deadline:{task.ProjectId}:{task.DeadLine:yyyyMMdd}");
                await _cacheService.RemoveAsync($"Tasks:name:{task.ProjectId}:{task.TaskName.ToLower()}");
                await _cacheService.RemoveAsync($"Tasks:status:{task.Status}:{task.ProjectId}");
                await _cacheService.RemoveAsync("Tasks:pagination");
                await _cacheService.RemoveAsync($"Tasks:pagination:{task.ProjectId}");
                await _cacheService.RemoveAsync($"Tasks:name::pagination:{task.ProjectId}:{task.TaskName.ToLower()}");

                var result = await _repository.DeleteTaskByIdAsync(task.Id, cancellationToken);

                if (result != null && result > 0)
                {
                    var project = await _projectRepository.GetProjectByIdAsync(task.ProjectId.Value, cancellationToken);
                    var userIds = project?.Users?.Select(u => u.Id).ToList() ?? new List<int>();
                    await _changeService.CreateChangeAsync(ChangeTableType.Tasks, task.Id, ChangeType.Deleted, userIds, cancellationToken);
                }

                return result != null && result > 0;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: DeleteTasksByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with DeleteTasksByIdAsync");
            }
        }
        //Method for getting all Tasks
        public async Task<ICollection<TaskGetDto>> GetAllTasksAsync(CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<TaskGetDto>>("Tasks");
                if (cache == null)
                {
                    var tasks = await _repository.GetAllTasksAsync(cancellationToken);
                    cache = _mapper.Map<ICollection<TaskGetDto>>(tasks);
                    await _cacheService.SetAsync("Tasks", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetAllTasksAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetAllTasksAsync");
            }
        }
        //Method for Task getting by deadline
        public async Task<TaskGetDto?> GetTaskByDeadLineAsync(DateTime date, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<TaskGetDto>($"Tasks:deadline:{projectId}:{date:yyyyMMdd}");
                if (cache == null)
                {
                    var task = await _repository.GetProjectTasksByDeadlineAsync(date, projectId, cancellationToken);
                    if (task == null) return null;
                    cache = _mapper.Map<TaskGetDto>(task);
                    await _cacheService.SetAsync($"Tasks:deadline:{projectId}:{date:yyyyMMdd}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTasksByDeadLineAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTasksByDeadLineAsync");
            }
        }
        //Method for Task getting by status
        public async Task<TaskGetDto?> GetTaskByStatusAsync(TaskFlow.Domain.Enums.TaskEnums.TaskStatus status, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<TaskGetDto>($"Tasks:status:{status}:{projectId}");
                if (cache == null)
                {
                    var task = await _repository.GetProjectTasksByStatusAsync(status, projectId, cancellationToken);
                    if (task == null) return null;
                    cache = _mapper.Map<TaskGetDto>(task);
                    await _cacheService.SetAsync($"Tasks:status:{status}:{projectId}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTaskByStatusAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTaskByStatusAsync");
            }
        }
        //Method for Task getting by id
        public async Task<TaskGetDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<TaskGetDto>($"Tasks:id:{id}");
                if (cache == null)
                {
                    var task = await _repository.GetTaskByIdAsync(id, cancellationToken);

                    if (task == null) return null;

                    cache = _mapper.Map<TaskGetDto>(task);

                    await _cacheService.SetAsync($"Tasks:id:{id}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTasksByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTasksByIdAsync");
            }
        }
        //Method for Task getting by name or part of name
        public async Task<ICollection<TaskGetDto?>> GetTaskByNameAsync(string name, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                var normalizedName = name.ToLower();
                var cache = await _cacheService.GetAsync<ICollection<TaskGetDto>>($"Tasks:name:{projectId}:{normalizedName}");
                if (cache == null)
                {
                    var task = await _repository.GetProjectTaskByNameAsync(name, projectId, cancellationToken);

                if (task == null) return null;

                cache = _mapper.Map<ICollection<TaskGetDto>>(task);

                await _cacheService.SetAsync($"Tasks:name:{projectId}:{normalizedName}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTasksByNameAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTasksByNameAsync");
            }
        }
        //Method for Task paginating
        public async Task<ICollection<TaskGetDto>> GetTasksPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<TaskGetDto>>($"Tasks:pagination:{count}:{side}");
                if (cache == null)
                {
                    ICollection<TaskEntity> tasks = await _repository.GetTasksPaginationAsync(count, side, cancellationToken);
                    cache = _mapper.Map<ICollection<TaskGetDto>>(tasks);
                    await _cacheService.SetAsync($"Tasks:pagination:{count}:{side}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTasksPagitationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTasksPagitationAsync");
            }
        }
        public async Task<ICollection<TaskGetDto>> GetProjectTasksPaginationAsync(int projectId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<TaskGetDto>>($"Tasks:pagination:{projectId}:{count}:{side}");
                if (cache == null)
                {
                    ICollection<TaskEntity> tasks = await _repository.GetProjectTasksPaginationAsync(projectId,count, side, cancellationToken);
                    cache = _mapper.Map<ICollection<TaskGetDto>>(tasks);
                    await _cacheService.SetAsync($"Tasks:pagination:{projectId}:{count}:{side}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTasksPagitationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTasksPagitationAsync");
            }
        }
        //Method for Task name paginating 
        public async Task<ICollection<TaskGetDto>> GetTasksByNamePaginationAsync(string name, int projectId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var normalizedName = name.ToLower();
                var cache = await _cacheService.GetAsync<ICollection<TaskGetDto>>($"Tasks:name::pagination:{projectId}:{normalizedName}:{count}:{side}");
                if (cache == null)
                {
                    var task = await _repository.GetProjectTaskByNamePaginationAsync(name, count, side, projectId, cancellationToken);

                    if (task == null) return null;

                    cache = _mapper.Map<ICollection<TaskGetDto>>(task);

                    await _cacheService.SetAsync($"Tasks:name:pagination:{projectId}:{normalizedName}:{count}:{side}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: GetTasksByNamePaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with GetTasksByNamePaginationAsync");
            }
        }
        //Method for Task updating by id
        public async Task<TaskGetDto?> UpdeteTaskAsync(int id, TaskUpdateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var existingTask = await _repository.GetTaskByIdAsync(id, cancellationToken);
                if (existingTask == null)
                    return null;

                var entity = _mapper.Map<TaskEntity>(dto);
                await _cacheService.RemoveAsync("Tasks");
                await _cacheService.RemoveAsync($"Tasks:id:{id}");
                await _cacheService.RemoveAsync($"Tasks:deadline:{entity.ProjectId}:{entity.DeadLine:yyyyMMdd}");
                await _cacheService.RemoveAsync($"Tasks:name:{entity.ProjectId}:{entity.TaskName.ToLower()}");
                await _cacheService.RemoveAsync($"Tasks:status:{entity.Status}:{entity.ProjectId}");
                await _cacheService.RemoveAsync("Tasks:pagination");
                await _cacheService.RemoveAsync($"Tasks:pagination:{entity.ProjectId}");
                await _cacheService.RemoveAsync($"Tasks:name::pagination:{entity.ProjectId}:{entity.TaskName.ToLower()}");
                await _repository.UpdateAsync(entity, cancellationToken);

                var updatedTask = await _repository.UpdateAsync(entity, cancellationToken);

                if (updatedTask != null)
                {
                    var project = await _projectRepository.GetProjectByIdAsync(existingTask.ProjectId.Value, cancellationToken);
                    var userIds = project?.Users?.Select(u => u.Id).ToList() ?? new List<int>();
                    await _changeService.CreateChangeAsync(ChangeTableType.Tasks, id, ChangeType.Updated, userIds, cancellationToken);
                }
                return _mapper.Map<TaskGetDto>(updatedTask);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: UpdateTasksByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with UpdateTasksByIdAsync");
            }
        }

        public async Task<ICollection<TaskGetDto>> GetProjectTasksAsync(int projectId, CancellationToken cancellationToken)
        {
            try
            {
                ICollection<TaskEntity> tasks = await _repository.GetTasksByProjectIdAsync(projectId, cancellationToken);
                return _mapper.Map<ICollection<TaskGetDto>>(tasks);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Sevice: UpdateTasksByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Service: Problem with UpdateTasksByIdAsync");
            }
        }
    }
}
