using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs
{
    /// <summary>
    /// Used to return project information.
    /// </summary>
    public class ProjectGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public ICollection<int> UsersId { get; set; }
        public ICollection<int> TasksId { get; set; }
    }
}
