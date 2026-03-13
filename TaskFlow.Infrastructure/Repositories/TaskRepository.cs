using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;
        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }
        public async Task<int?> AddTaskAsync(TaskEntity task, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Tasks.AddAsync(task);
                return task.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: AddTaskAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with AddTaskAsync");
            }
        }

        public async Task<int?> DeleteTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _context.Tasks.Remove(new TaskEntity { Id = id});
                await _context.SaveChangesAsync(cancellationToken);
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: DeleteTaskByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with DeleteTaskByIdAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetAllTasksAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .ToListAsync();
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetAllTasksAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetAllTasksAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetProjectTaskByNameAsync(string name, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .Where(t => t.ProjectId == projectId)
                    .Where(t => t.TaskName.Contains(name))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetProjectTaskByNameAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetProjectTaskByNameAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetProjectTaskByNamePaginationAsync(string name, int projectId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .OrderBy(t => t.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .Where(t => t.ProjectId == projectId)
                    .Where(t => t.TaskName.Contains(name))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetProjectTaskByNamePaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetProjectTaskByNamePaginationAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetProjectTasksByDeadlineAsync(DateTime dueDate, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .Where(t => t.ProjectId == projectId)
                    .Where(t => t.DeadLine == dueDate)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetProjectTasksByDeadlineAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetProjectTasksByDeadlineAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetProjectTasksByStatusAsync(TaskFlow.Domain.Enums.TaskEnums.TaskStatus status, int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include (t => t.User)
                    .Where (t => t.ProjectId == projectId)
                    .Where(t => t.Status == status)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetProjectTasksByStatusAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetProjectTasksByStatusAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetProjectTasksPaginationAsync(int projectId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include (t => t.User)
                    .OrderBy(t => t.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .Where(t => t.ProjectId == projectId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetProjectTasksPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetProjectTasksPaginationAsync");
            }
        }

        public async Task<TaskEntity?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include (t => t.User)
                    .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetTaskByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetTaskByIdAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetTasksByProjectIdAsync(int projectId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.User)
                    .Where (t => t.ProjectId == projectId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetTasksByProjectIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetTasksByProjectIdAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetTasksByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tasks
                    .Include (t => t.User)
                    .Include(t => t.Project)
                    .Where(t => t.UserId == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetTasksByUserIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetTasksByUserIdAsync");
            }
        }

        public async Task<ICollection<TaskEntity>?> GetTasksPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await _context.Tasks
                    .Include (t => t.User)
                    .Include(t => t.Project)
                    .OrderBy(t => t.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync (cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: GetTasksPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with GetTasksPaginationAsync");
            }
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Task Repository: UpdateAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("Task Repository: Problem with UpdateAsync");
            }
        }
    }
}
