using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs.ListProjectDTOs
{
    /// <summary>
    /// Використовується для відображення проекту у списках.
    /// Містить інформацію про кількість задач, кількість учасників.
    /// Не містить детальних даних проекту.
    /// </summary>
    public class ProjectListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int WorkersCount { get; set; }
        public int TasksCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
