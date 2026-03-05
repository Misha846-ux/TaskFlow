using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.ProjectDTOs.CreateProjectDTOs
{
    /// <summary>
    /// Використовується для додавання користувача до проекту.
    /// Передається в endpoint додавання учасника.
    /// Містить Id користувача та його роль у межах проекту.
    /// </summary>
    public class AddUserToProjectDto
    {
        public int UserId { get; set; }
        public ProjectRole ProjectRole { get; set; }
    }
}
