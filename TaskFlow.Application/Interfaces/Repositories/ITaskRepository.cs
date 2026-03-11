using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<ICollection<TaskEntity>?> GetAllTasksAsync();
    Task<ICollection<TaskEntity>?> GetTasksByProjectIdAsync(int projectId);
    Task<ICollection<TaskEntity>?> GetTasksPaginationAsync(int count, int side);
    Task<ICollection<TaskEntity>?> GetProjectTasksPaginationAsync(int projectId, int count, int side);
    Task<TaskEntity?> GetTaskByIdAsync(int id);
    Task<ICollection<TaskEntity>?> GetProjectTaskByNameAsync(string name, int projectId); // There can be tasks with the same name so we need to return a collection of tasks but I think it is better to make this method without pagination because we can get all tasks with the same name and then paginate them on the client side if needed
    Task<ICollection<TaskEntity>?> GetProjectTaskByNamePaginationAsync(string name, int projectId, int count, int side);
    Task<ICollection<TaskEntity>?> GetProjectTasksByDeadlineAsync(DateTime dueDate, int projectId);
    Task<int?> DeleteTaskByIdAsync(int id);
    Task<int?> UpdateTaskByIdAsync();
    Task<int?> AddTaskAsync(TaskEntity task);
    Task<ICollection<TaskEntity>?> GetTasksByUserIdAsync(int userId); // I think it is better to make this method without pagination because user does not have so many tasks
    Task<ICollection<TaskEntity>?> GetProjectTasksByStatusAsync(TaskStatus status, int projectId);
    
}
