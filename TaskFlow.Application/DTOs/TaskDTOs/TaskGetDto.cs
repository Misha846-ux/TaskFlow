using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums.TasksEnums;

namespace TaskFlow.Application.DTOs.TaskDTOs;

public class TaskGetDto
{
    public int Id { get; set; }
    public TaskStatus Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }
}
