using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ComapniesDTOs
{
    /// <summary>
    /// Used to return a response to the user.
    /// </summary>
    public class CompanyGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<int> ProjectsId { get; set; }
        public ICollection<int> EmploeesId { get; set; }
    }
}
