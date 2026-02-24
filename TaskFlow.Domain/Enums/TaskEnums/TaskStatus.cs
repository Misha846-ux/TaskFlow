using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Enums.TaskEnums;

internal enum TaskStatus
{
    Waiting = 0,
    InProgress = 1,
    Completed = 2,
}
