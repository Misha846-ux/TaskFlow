using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class ChangeRepository : IChangeRepository
    {
        private readonly TaskFlowDbContext _context;

        public ChangeRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ChangeEntity>> GetChangesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Changes
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Changes by User Id", ex);
            }
        }

        public async Task<ICollection<ChangeEntity>> GetChangesByUserIdPaginatedAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Changes
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Get Changes by User Id Paginated", ex);
            }
        }

        public async Task CreateChangesAsync(List<ChangeEntity> changes, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Changes.AddRangeAsync(changes, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("Operation was canceled", oex);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem with Create Changes", ex);
            }
        }
    }
}