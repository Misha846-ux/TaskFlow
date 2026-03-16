using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDto;

public class TaskPostDto
{
    public string TaskName { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public DateTime DeadLine { get; set; }
    public int ProjectId { get; set; }
}
