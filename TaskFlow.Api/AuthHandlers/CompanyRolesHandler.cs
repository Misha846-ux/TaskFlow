using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Application.AuthorizationRequirements;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.AuthHandlers
{
    public class CompanyRolesHandler : AuthorizationHandler<CompanyRequirementRoles>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyRequirementRoles requirement)
        {
            var companyRoleClaim = context.User.Claims.FirstOrDefault(c => c.Type == nameof(CompanyRole))?.Value;
            if (companyRoleClaim == null)
                return Task.CompletedTask;

            if (!Enum.TryParse(companyRoleClaim, true, out CompanyRole userRole))
                return Task.CompletedTask;

            if (requirement.AllowedRoles.Contains(userRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
