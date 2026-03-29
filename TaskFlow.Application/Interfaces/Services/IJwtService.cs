using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserEntity user);
        Task<RefreshTokenEntity> GenerateRefreshTokenAsync(int userId);
        Task<RefreshTokenEntity?> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);
        ClaimsPrincipal? DecodeToken(string token);
        string? GetTokenUsersId(string JwtToken);
    }
}
