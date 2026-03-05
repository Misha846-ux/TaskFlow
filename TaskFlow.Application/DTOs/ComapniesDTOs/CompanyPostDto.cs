using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ComapniesDTOs
{
    /// <summary>
    /// Used to obtain data for creating a company.
    /// </summary>
    public class CompanyPostDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
