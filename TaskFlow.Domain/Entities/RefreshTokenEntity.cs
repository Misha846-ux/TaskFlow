using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Token { get; set; }
        [Required]
        public DateTime? Expires { get; set; }
        [Required]
        public bool? IsRevoked { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; }
        [Required]
        public int? UserId { get; set; }
        public UserEntity User { get; set; }
    }
}