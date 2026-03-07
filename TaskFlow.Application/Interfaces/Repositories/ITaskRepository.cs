using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<ICollection<TaskEntity>?> GetAllTaskAsync();
    Task<TaskEntity?> GetTaskByIdAsync(int id);
    Task<TaskEntity?> GetTaskByName(string name);
    Task<int?> DeleteTaskByIdAsync(int id);
    Task<TaskEntity> UpdateTaskByIdAsync(int id, TaskEntity task);
    Task<int?> AddTaskAsync(TaskEntity task);
}
