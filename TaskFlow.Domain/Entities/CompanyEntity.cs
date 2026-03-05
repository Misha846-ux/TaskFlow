using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ProjectUserEntity> Projects { get; set; }
        public ICollection<CompanyUserEntity> Companies { get; set; }
    }
}
