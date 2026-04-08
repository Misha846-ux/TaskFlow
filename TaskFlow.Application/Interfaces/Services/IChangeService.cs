using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ChangeDTOs;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IChangeService
    {
        /// <summary>
        /// Returns all changes for the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ChangeDto>> GetChangesByUserIdAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns only unread changes for the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ChangeDto>> GetUnreadChangesByUserIdAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns changes for the specified user with pagination.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="side"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ChangeDto>> GetChangesByUserIdPaginatedAsync(int userId, int count, int side, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new change for the specified users.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="noteId"></param>
        /// <param name="changeType"></param>
        /// <param name="userIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateChangeAsync(ChangeTableType table, int noteId, ChangeType changeType, List<int> userIds, CancellationToken cancellationToken);
    }
}