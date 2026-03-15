using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        [Required]
        public int? CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
        public ICollection<ProjectUserEntity> Users { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }
    }
}
