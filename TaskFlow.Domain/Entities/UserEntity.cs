using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public GlobalRole GlobalRole { get; set; } = GlobalRole.User;
        public string PassToIcon { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Settings { get; set; } = string.Empty;
        public string RecoveryToken {  get; set; }
        public DateTime RecoveryTokenLifeTime { get; set; } 
        public ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<ProjectUserEntity> Projects { get; set; }
        public ICollection<CompanyUserEntity> Companies { get; set; }
        public ICollection<ChangeEntity> Changes { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}
