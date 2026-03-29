using Microsoft.AspNetCore.Authorization;
using TaskFlow.Application.AuthorizationRequirements;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Api.AuthHandlers
{
    public class CompanyRolesHandler : AuthorizationHandler<CompanyRequirementRoles>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyRequirementRoles requirement)
        {
            var userRolesClaim = context.User.Claims.SingleOrDefault(c => c.Type == nameof(CompanyRole))?.Value;
            if (userRolesClaim == null)
            {
                return Task.CompletedTask;
            }
            Enum.TryParse(userRolesClaim, out CompanyRole userRole);

            bool roleMatch = requirement.AllowedRoles.Any(r => r.HasFlag(userRole));
            if (roleMatch)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
