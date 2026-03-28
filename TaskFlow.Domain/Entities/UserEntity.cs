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
        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Required]
        public GlobalRole? GlobalRole { get; set; }
        public string? PassToIcon { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        public string? Settings { get; set; }
        public string? RecoveryTokenHash {  get; set; }
        public DateTime? RecoveryTokenLifeTime { get; set; } 
        public ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<ProjectEntity> Projects { get; set; }
        public ICollection<CompanyUserEntity> Companies { get; set; }
        public ICollection<ChangeEntity> Changes { get; set; }
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}
