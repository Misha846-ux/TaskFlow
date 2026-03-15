using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class CompanyUserEntity
    {
        public int Id { get; set; }
        [Required]
        public int? CompanyId { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public CompanyRole? CompanyRole { get; set; }
        public CompanyEntity Company { get; set; }
        public UserEntity User { get; set; }
    }
}
