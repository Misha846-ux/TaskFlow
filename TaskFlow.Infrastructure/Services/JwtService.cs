using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly TaskFlowDbContext _context;

        public JwtService(
            IConfiguration configuration,
            TaskFlowDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateAccessToken(UserEntity user)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshTokenEntity> GenerateRefreshTokenAsync(int userId)
        {
            var expirationDays = int.Parse(
                _configuration["Jwt:RefreshTokenExpirationDays"]);

            var token = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshTokenEntity
            {
                Token = token,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(expirationDays),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshTokenEntity?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (refreshToken == null)
                return;

            refreshToken.IsRevoked = true;

            await _context.SaveChangesAsync();
        }
    }
}
