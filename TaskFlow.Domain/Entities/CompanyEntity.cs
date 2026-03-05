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
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
        public virtual ICollection<CompanyUserEntity> CompanyUsers { get; set; } = new List<CompanyUserEntity>();
    }
}
