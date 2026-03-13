using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
        public ICollection<ProjectUserEntity> Users { get; set; } = new List<ProjectUserEntity>();
    }
}
