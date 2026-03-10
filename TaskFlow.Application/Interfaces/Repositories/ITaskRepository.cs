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
    Task<ICollection<TaskEntity>?> GetTasksPaginationAsync(int count, int side);
    Task<TaskEntity?> GetTaskByIdAsync(int id);
    Task<TaskEntity?> GetTaskByNameAsync(string name); // There can be tasks with the same name so we need to return a collection of tasks but I think it is better to make this method without pagination because we can get all tasks with the same name and then paginate them on the client side if needed
    Task<ICollection<TaskEntity>?> GetTasksByDeadlineAsync(DateTime dueDate);
    Task<int?> DeleteTaskByIdAsync(int id);
    Task<int?> UpdateTaskByIdAsync(int id, TaskEntity task);
    Task<int?> AddTaskAsync(TaskEntity task);
    Task<ICollection<TaskEntity>?> GetTasksByUserIdAsync(int userId); // I think it is better to make this method without pagination because user does not have so many tasks

    //There are no such methods in controllers but maybe we could add them because it is esier to work with them and make server for tasks more empty and clean

    //Task<ICollection<TaskEntity>?> GetTasksByStatusAsync(string status);
    //Task<ICollection<TaskEntity>?> GetTasksByProjectIdAsync(int projectId);
}
