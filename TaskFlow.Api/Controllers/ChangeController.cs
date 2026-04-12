using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.ChangeDTOs;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/changes")]
    [Authorize]
    public class ChangeController : ControllerBase
    {
        private readonly IChangeService _changeService;

        public ChangeController(IChangeService changeService)
        {
            _changeService = changeService;
        }

        /// <summary>
        /// Get all changes for the current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetChangesForUser(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var changes = await _changeService.GetChangesByUserIdAsync(userId, cancellationToken);
            return Ok(changes);
        }

        /// <summary>
        /// Get only unread changes for the current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadChangesForUser(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var changes = await _changeService.GetUnreadChangesByUserIdAsync(userId, cancellationToken);
            return Ok(changes);
        }

        /// <summary>
        /// Get changes for the current user with pagination
        /// </summary>
        /// <param name="count">Number of changes per page</param>
        /// <param name="side">Page number (1-indexed)</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("paginated")]
        public async Task<IActionResult> GetChangesPaginated([FromQuery] int count = 10, [FromQuery] int side = 1, CancellationToken cancellationToken = default)
        {
            var userId = GetUserId();
            var changes = await _changeService.GetChangesByUserIdPaginatedAsync(userId, count, side, cancellationToken);
            return Ok(changes);
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedException("User ID not found.");
            }
            return userId;
        }
    }
}