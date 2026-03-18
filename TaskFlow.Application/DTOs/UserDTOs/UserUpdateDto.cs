using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; } = null;
        [EmailAddress]
        public string? Email { get; set; } = null;
        public int? GlobaleRole { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? PassToIcon { get; set; } = null;
        public string? Settings { get; set; } = null;
    }
}
