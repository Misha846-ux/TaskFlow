using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ProjectDTOs
{
    /// <summary>
    /// Used to retrieve information about project updates.
    /// Fields that do not need to be changed are specified as null.
    /// </summary>
    public class ProjectUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
