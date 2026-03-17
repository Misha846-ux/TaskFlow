using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TaskFlow.Domain.Enums.TaskEnums;

namespace TaskFlow.Domain.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        [Required]
        public Enums.TaskEnums.TaskStatus? Status { get; set; }
        [Required]
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        [Required]
        public TaskPriority? Priority { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        [Required]
        public DateTime? DeadLine { get; set; }
        [Required]
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }    
        public ProjectEntity Project { get; set; }
        public UserEntity User { get; set; }
    }
}
