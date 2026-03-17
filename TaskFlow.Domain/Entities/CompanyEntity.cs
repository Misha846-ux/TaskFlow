using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        public ICollection<ProjectEntity> Projects { get; set; }
        public ICollection<CompanyUserEntity> Users { get; set; }
    }
}
