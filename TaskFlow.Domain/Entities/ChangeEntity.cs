using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class ChangeEntity
    {
        public int Id { get; set; }
        [Required]
        public ChangeTableType? Table { get; set; }
        [Required]
        public int? NoteId { get; set; }
        [Required]
        public int? UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
