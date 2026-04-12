using System;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.ChangeDTOs
{
    public class ChangeDto
    {
        public int Id { get; set; }
        public ChangeTableType? Table { get; set; }
        public int? EntityId { get; set; }
        public ChangeType? ChangeType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Message { get; set; }
    }
}
