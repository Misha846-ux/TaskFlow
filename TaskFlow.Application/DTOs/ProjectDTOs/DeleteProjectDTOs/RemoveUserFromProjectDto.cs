using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs.DeleteProjectDTOs
{
    /// <summary>
    /// Використовується для видалення користувача з проекту.
    /// Передається у DELETE endpoint.
    /// </summary>
    public class RemoveUserFromProjectDto
    {
        [Required]
        public int UserId { get; set; }
    }
}
