using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs.DetailsProjectDTOs
{
    /// <summary>
    /// Використовується для фільтрації та пагінації.
    /// Дозволяє реалізувати пошук, фільтр по компанії та пагінацію.
    /// </summary>
    public class ProjectFilterDto
    {
        public int? CompanyId { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
