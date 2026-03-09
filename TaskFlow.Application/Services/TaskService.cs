using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDto;
using TaskFlow.Application.DTOs.TaskDTOs;
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
        public Task<int?> CreateTaskAsync(TaskPostDto dto)
        {
            await _cacheService.RemoveAsync("Tasks");

            var task = _mapper.Map<TaskEntity>(dto);

            return await _repository.AddTaskAsync(book, dto.ProjectId);
        }
        //Method for Task deleting by id
        public Task<bool> DeleteTaskByIdAsync(int id)
        {
            await _cacheService.RemoveAsync("Tasks");

            var task = await _repository.GetTaskByIdAsync(id);

            if (task == null)
                return false;

            var result = await _repository.DeleteTaskByIdAsync(task);

            return result != null && result > 0;
        }
        //Method for getting all Tasks
        public Task<ICollection<TaskGetDto>> GetAllTasksAsync()
        {
            var cache = await _cacheService.GetAsync<ICollection<TaskGetDto>>("Tasks");
            if (cache == null)
            {
                var tasks = await _repository.GetAllTasksAsync();
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
        public Task<TaskGetDto?> GetTaskByDeadLineAsync(DateTime date)
        {
            var task = await _repository.GetTaskByDeadLineAsync(date);

            if (task == null) return null;

            var dto = _mapper.Map<TaskGetDto>(task);

            return dto;
        }
        //Method for Task getting by id
        public Task<TaskGetDto?> GetTaskByIdAsync(int id)
        {
            var task = await _repository.GetTaskByIdAsync(id);

            if (task == null) return null;

            var dto = _mapper.Map<TaskGetDto>(task);

            return dto;
        }
        //Method for Task getting by name or part of name
        public Task<ICollection<TaskGetDto?>> GetTaskByNameAsync(string name)
        {
            var task = await _repository.GetTaskByNameAsync(name);

            if (task == null) return null;

            var dto = _mapper.Map<TaskGetDto>(task);

            return dto;
        }
        //Method for Task updating by id
        public Task<TaskGetDto?> UpdeteTaskAsync(int id, TaskUpdateDto dto)
        {
            await _cacheService.RemoveAsync("Tasks");
            var entity = _mapper.Map<TaskEntity>(dto);
            var update = await _repository.UpdeteTaskById(id, entity);

            if (update == null) return null;

            return _mapper.Map<TaskGetDto>(update);
        }
    }
}
