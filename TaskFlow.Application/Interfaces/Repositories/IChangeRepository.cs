using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IChangeRepository
    {
        Task<ICollection<ChangeEntity>> GetChangesByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<ICollection<ChangeEntity>> GetChangesByUserIdPaginatedAsync(int userId, int count, int side, CancellationToken cancellationToken);
        Task CreateChangesAsync(List<ChangeEntity> changes, CancellationToken cancellationToken);
    }
}
