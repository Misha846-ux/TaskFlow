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
            await _cacheService.RemoveAsync("Tasks");

            var task = _mapper.Map<TaskEntity>(dto);

            return await _repository.AddTaskAsync(task, cancellationToken);
        }
        //Method for Task deleting by id
        public async Task<bool> DeleteTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync("Tasks");

            var task = await _repository.GetTaskByIdAsync(id, cancellationToken);

            if (task == null)
                return false;

            var result = await _repository.DeleteTaskByIdAsync(task.Id, cancellationToken);

            return result != null && result > 0;
        }
        //Method for getting all Tasks
        public async Task<ICollection<TaskGetDto>> GetAllTasksAsync(CancellationToken cancellationToken)
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
        //Method for Task paginating
        //public Task<ICollection<TaskGetDto>> GetChunkAsync(int pagenum, int limit)
        //{
        //    var tasks = await _repository.GetChunk(pagenum, limit);
        //    return _mapper.Map<ICollection<TaskGetDto>>(tasks);
        //}
        //Method for Task getting by deadline
        public async Task<TaskGetDto?> GetTaskByDeadLineAsync(DateTime date, int projectId, CancellationToken cancellationToken)
        {
            var task = await _repository.GetProjectTasksByDeadlineAsync(date, projectId, cancellationToken);

            if (task == null) return null;

            var dto = _mapper.Map<TaskGetDto>(task);

            return dto;
        }
        //Method for Task getting by id
        public async Task<TaskGetDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            var task = await _repository.GetTaskByIdAsync(id, cancellationToken);

            if (task == null) return null;

            var dto = _mapper.Map<TaskGetDto>(task);

            return dto;
        }
        //Method for Task getting by name or part of name
        public async Task<ICollection<TaskGetDto?>> GetTaskByNameAsync(string name, int projectId, CancellationToken cancellationToken)
        {
            var task = await _repository.GetProjectTaskByNameAsync(name, projectId, cancellationToken);

            if (task == null) return null;

            var dto = _mapper.Map<ICollection<TaskGetDto>>(task);

            return dto;
        }
        //Method for Task updating by id
        public async Task<TaskGetDto?> UpdeteTaskAsync(int id, TaskUpdateDto dto, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync("Tasks");
            var entity = _mapper.Map<TaskEntity>(dto);
            await _repository.UpdateAsync(entity, cancellationToken);


            return _mapper.Map<TaskGetDto>(entity);
        }
    }
}
