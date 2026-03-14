using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.AuthDTOs;
using TaskFlow.Application.DTOs.UserDTOs;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(UserPostDto dto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task ForgotPasswordAsync(string email);
    }
}
