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
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public int Workers { get; set; }
        public virtual CompanyEntity Company { get; set; } = null!;
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
        public virtual ICollection<ProjectUserEntity> ProjectUsers { get; set; } = new List<ProjectUserEntity>();
    }
}
