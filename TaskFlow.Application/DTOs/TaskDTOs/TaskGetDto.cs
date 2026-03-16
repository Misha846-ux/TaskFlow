using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDTOs;

public class TaskGetDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string TaskName { get; set; } 
    public string Description { get; set; } 
    public string Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeadLine { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }
}
