using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs
{
    /// <summary>
    /// Used to obtain data for creating a project.
    /// </summary>
    public class ProjectPostDto
    {
        public string Title { get; set; }
        public int CompanyId { get; set; }
    }
}
