using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class ProjectUserEntity
    {
        public int Id { get; set; }
        [Required]
        public int? ProjectId { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public ProjectRole? ProjectRole { get; set; }
        public ProjectEntity Project { get; set; }
        public UserEntity User { get; set; }
    }
}
