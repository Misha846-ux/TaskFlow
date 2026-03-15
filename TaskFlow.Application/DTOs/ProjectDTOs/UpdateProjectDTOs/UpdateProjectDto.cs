using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int id {  get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = null!;
    }
}
