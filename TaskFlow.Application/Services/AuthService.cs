using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.AuthDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Helpers;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHashHelper _hashHelper;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IHashHelper hashHelper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _hashHelper = hashHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserPostDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email, CancellationToken.None);
            if (existingUser != null)
                throw new Exception("Email already exists");

            var user = new UserEntity
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = _hashHelper.Hash(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(user, dto.Password, CancellationToken.None);

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email, CancellationToken.None);
            if (user == null || !_hashHelper.IsValidPassword(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.Expires <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            storedToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            var user = storedToken.User!;
            var tokens = await GenerateTokensAsync(user);
            return tokens;
        }

        public Task ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(UserEntity user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
