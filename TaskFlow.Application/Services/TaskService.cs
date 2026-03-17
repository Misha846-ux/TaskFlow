using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDto;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskFlow.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICachingService _cacheService;
        public TaskService(ITaskRepository repository, IMapper mapper, ICachingService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        //Method for Task creating
        public async Task<int?> CreateTaskAsync(TaskPostDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _cacheService.RemoveAsync("Tasks");
                await _cacheService.RemoveAsync($"Tasks:deadline:{dto.ProjectId}:{dto.DeadLine:yyyyMMdd}");
                await _cacheService.RemoveAsync($"Tasks:name:{dto.ProjectId}:{dto.TaskName.ToLower()}");
                var task = _mapper.Map<TaskEntity>(dto);

                return await _repository.AddTaskAsync(task, cancellationToken);
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

                var result = await _repository.DeleteTaskByIdAsync(task.Id, cancellationToken);

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
        //Method for Task paginating
        //public Task<ICollection<TaskGetDto>> GetChunkAsync(int pagenum, int limit)
        //{
        //    var tasks = await _repository.GetChunk(pagenum, limit);
        //    return _mapper.Map<ICollection<TaskGetDto>>(tasks);
        //}
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
                    await _cacheService.SetAsync($"Tasks:deadline:{projectId}:{date}", cache, null);
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

                await _cacheService.SetAsync($"Tasks:name:{projectId}:{name}", cache, null);
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
        //Method for Task updating by id
        public async Task<TaskGetDto?> UpdeteTaskAsync(int id, TaskUpdateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<TaskEntity>(dto);
                await _cacheService.RemoveAsync("Tasks");
                await _cacheService.RemoveAsync($"Tasks:id:{id}");
                await _cacheService.RemoveAsync($"Tasks:deadline:{entity.ProjectId} : {entity.DeadLine:yyyyMMdd}");
                await _cacheService.RemoveAsync($"Tasks:name:{entity.ProjectId}:{entity.TaskName.ToLower()}");
                await _repository.UpdateAsync(entity, cancellationToken);


                return _mapper.Map<TaskGetDto>(entity);
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
