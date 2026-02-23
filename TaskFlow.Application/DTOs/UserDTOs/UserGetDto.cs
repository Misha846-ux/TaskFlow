using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.UserDTOs
{
    public class UserGetDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string GlobalRole { get; set; }
        public string PassTolcon {  get; set; }
        public string GreatedAt { get; set; }
        public byte[] CompaniesId { get; set; }
    }
}
