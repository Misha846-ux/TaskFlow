using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs
{
    /// <summary>
    /// Використовується для відображення учасників проекту.
    /// Містить базову інформацію про користувача та його роль.
    /// </summary>
    public class ProjectUserListItemDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public ProjectRole ProjectRole { get; set; }
    }
}
