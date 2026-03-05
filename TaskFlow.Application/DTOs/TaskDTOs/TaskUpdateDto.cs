using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums.TasksEnums;

namespace TaskFlow.Application.DTOs.TaskDTOs;

public class TaskUpdateDto
{
    public int Id { get; set; }
    public string? TaskName { get; set; } = null;
    public string? Description { get; set; } = null;
    public int? Status { get; set; } = null;
    public int? Priority { get; set; } = null;
    public DateTime? DeadLine { get; set; } = null;
    public int? UserId { get; set; } = null;
}
