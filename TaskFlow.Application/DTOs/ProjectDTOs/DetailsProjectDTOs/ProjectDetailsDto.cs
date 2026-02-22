using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs;

namespace TaskFlow.Application.DTOs.ProjectDTOs.DetailsProjectDTOs
{
    /// <summary>
    /// Використовується для відображення детальної інформації про проект.
    /// Містить повні дані проекту та список учасників.
    /// Не використовується для створення або оновлення.
    /// </summary>
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int WorkersCount { get; set; }
        public int TasksCount { get; set; }
        public List<ProjectUserListItemDto> Users { get; set; } = new();
    }
}
