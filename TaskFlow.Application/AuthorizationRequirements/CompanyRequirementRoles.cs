using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.AuthorizationRequirements
{
    public class CompanyRequirementRoles: IAuthorizationRequirement
    {
        public CompanyRole[] AllowedRoles { get; }
        public CompanyRequirementRoles(CompanyRole[] allowedRoles)
        {
            this.AllowedRoles = allowedRoles;
        }
    }
}
