using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public Task<int?> AddTaskAsync(TaskEntity task)
        {
            throw new NotImplementedException();
        }

        public Task<int?> DeleteTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetAllTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetProjectTaskByNameAsync(string name, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetProjectTaskByNamePaginationAsync(string name, int projectId, int count, int side)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetProjectTasksByDeadlineAsync(DateTime dueDate, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetProjectTasksByStatusAsync(TaskStatus status, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetProjectTasksPaginationAsync(int projectId, int count, int side)
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity?> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetTasksByProjectIdAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetTasksByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TaskEntity>?> GetTasksPaginationAsync(int count, int side)
        {
            throw new NotImplementedException();
        }

        public Task<int?> UpdateTaskByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
