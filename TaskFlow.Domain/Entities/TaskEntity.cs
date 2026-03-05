using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums.TasksEnums;

namespace TaskFlow.Domain.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public TaskStatus Status { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeadLine { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ProjectEntity Project { get; set; }
        public UserEntity User { get; set; }
    }
}
