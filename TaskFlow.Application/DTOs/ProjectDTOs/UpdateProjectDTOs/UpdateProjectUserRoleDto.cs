using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.ProjectDTOs.UpdateProjectDTOs
{
    /// <summary>
    /// Використовується для зміни ролі користувача в проекті.
    /// Передається при оновленні прав доступу учасника.
    /// </summary>
    public class UpdateProjectUserRoleDto
    {
        public ProjectRole ProjectRole { get; set; }
    }
}
