using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TaskFlowDbContext _context;

        public RefreshTokenRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshTokenEntity> AddAsync(RefreshTokenEntity token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<RefreshTokenEntity?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (refreshToken == null)
            {
                return;
            }
            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }
}
