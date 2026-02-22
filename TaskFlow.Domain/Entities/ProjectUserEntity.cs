using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class ProjectUserEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public virtual ProjectEntity Project { get; set; } = null!;
        public virtual UserEntity User { get; set; } = null!;
    }
}
