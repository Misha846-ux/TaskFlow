using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.UserDTOs
{
    public class UserGetDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string GlobalRole { get; set; }
        public string PassToIcon {  get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<int> CompaniesId { get; set; }
        public string Settings {  get; set; }
    }
}
