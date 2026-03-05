using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public TaskStatus Status { get; set; }
        public string TaskName { get; set; } = null!;
        public string? Description { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeadLine { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public virtual ProjectEntity Project { get; set; } = null!;
        public virtual UserEntity User { get; set; } = null!;
    }
}
