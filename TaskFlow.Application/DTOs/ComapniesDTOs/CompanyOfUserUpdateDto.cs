using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs.ComapniesDTOs;

public class CompanyOfUserUpdateDto
{
    public int Id { get; set; }
    public int? CompanyId { get; set; }
    public int? UserId { get; set; }
    public CompanyRole? CompanyRole { get; set; }
}
