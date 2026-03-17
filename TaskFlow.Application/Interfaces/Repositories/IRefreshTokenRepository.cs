using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenEntity> AddAsync(RefreshTokenEntity token);
        Task<RefreshTokenEntity?> GetByTokenAsync(string token);
        Task RevokeAsync(string token);
    }
}
