using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums.TasksEnums;

namespace TaskFlow.Application.DTOs.TaskDto;

public class TaskPostDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
}
