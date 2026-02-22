using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs.UpdateProjectDTOs
{
    /// <summary>
    /// Використовується для оновлення основної інформації проекту.
    /// Передається з клієнта (PATCH /...).
    /// Не містить пов'язаних сутностей або навігаційних даних.
    /// </summary>
    public class UpdateProjectDto
    {
        public string Title { get; set; } = null!;
    }
}
