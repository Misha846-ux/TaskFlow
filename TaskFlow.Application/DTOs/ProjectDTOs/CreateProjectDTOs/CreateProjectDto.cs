using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs.CreateProjectDTOs
{
    /// <summary>
    /// Використовується для створення нового проекту.
    /// Передається з клієнта (POST /...).
    /// Містить мінімальний набір даних, необхідний для створення проекту.
    /// </summary>
    public class CreateProjectDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = null!;
        [Required]
        public int CompanyId { get; set; }
    }
}
