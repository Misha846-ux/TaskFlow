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
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public GlobalRole GlobalRole { get; set; }
        public string PassToIcon { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Settings { get; set; } = string.Empty;
        public ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<ProjectUserEntity> Projects { get; set; }
        public ICollection<CompanyUserEntity> Companies { get; set; }
        public ICollection<ChangeEntity> Changes { get; set; }
    }
}
