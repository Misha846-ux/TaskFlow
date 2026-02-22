using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public GlobalRole GlobalRole { get; set; }
        public string? PassToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Settings { get; set; }
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
        public virtual ICollection<ProjectUserEntity> ProjectUsers { get; set; } = new List<ProjectUserEntity>();
        public virtual ICollection<CompanyUserEntity> CompanyUsers { get; set; } = new List<CompanyUserEntity>();
        public virtual ICollection<ChangeEntity> Changes { get; set; } = new List<ChangeEntity>();
    }
}
