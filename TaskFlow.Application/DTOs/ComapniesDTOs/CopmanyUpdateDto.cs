using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ComapniesDTOs
{
    /// <summary>
    /// Used to retrieve information about company name or description updates.
    /// Fields that do not need to be changed are specified as null.
    /// </summary>
    public class CopmanyUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
