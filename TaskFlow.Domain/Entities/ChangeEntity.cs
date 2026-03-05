using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class ChangeEntity
    {
        public int Id { get; set; }
        public ChangeTableType Table { get; set; }
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; } = null!;
    }
}
