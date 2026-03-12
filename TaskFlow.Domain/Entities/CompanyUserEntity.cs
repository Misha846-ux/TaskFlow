using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities
{
    public class CompanyUserEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public CompanyRole CompanyRole { get; set; } = CompanyRole.Employee;
        public CompanyEntity Company { get; set; }
        public UserEntity User { get; set; }
    }
}
