using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDto;
using TaskFlow.Application.DTOs.TaskDTOs;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface ITaskService
    {
        /// <summary>
        /// Returns all Tasks
        /// </summary>
        /// <returns>Collection TaskGetDto</returns>
        Task<ICollection<TaskGetDto>> GetAllTasksAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Return Task by id
        /// </summary>
        /// <param name="id">Id of Task</param>
        /// <returns>TaskGetDto or null</returns>
        Task<TaskGetDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>
        /// Return Task by name or piece of name
        /// </summary>
        /// <param name="name">Name of Task</param>
        /// <returns>TaskGetDto or null</returns>
        Task<ICollection<TaskGetDto?>> GetTaskByNameAsync(string name, int projectId, CancellationToken cancellationToken);
        /// <summary>
        /// Return Task by date of DeadLine
        /// </summary>
        /// <param name="date">DeadLine of Task</param>
        /// <param name="porjectId">The project ID to which the task should belong</param>
        /// <returns>TaskGetDto or null</returns>
        Task<TaskGetDto?> GetTaskByDeadLineAsync(DateTime date, int porjectId, CancellationToken cancellationToken);
        /// <summary>
        /// Create new Task
        /// </summary>
        /// <param name="dto">TaskPostDto for Task's creating</param>
        /// <returns>Object and Id of new Task</returns>
        Task<int?> CreateTaskAsync(TaskPostDto dto, CancellationToken cancellationToken);
        /// <summary>
        /// Delete Task by id
        /// </summary>
        /// <param name="id">Id of Task</param>
        /// <returns>True or False</returns>
        Task<bool> DeleteTaskByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>
        /// Update Task by id
        /// </summary>
        /// <param name="id">Id of Task</param>
        /// <param name="dto">TaskUpdateDto  for Task's updating</param>
        /// <returns>Object of updated Task</returns>
        Task<TaskGetDto?> UpdeteTaskAsync(int id, TaskUpdateDto dto, CancellationToken cancellationToken);
        /// <summary>
        ///     Make Task Pagination
        /// </summary>
        /// <param name="pagenum">Count of Task's blocks</param>
        /// <param name="limit">Limit of Task's amount</param>
        /// <returns>Objects of Tasks</returns>
        //Task<ICollection<TaskGetDto>> GetChunkAsync(int pagenum, int limit);
    }
}
