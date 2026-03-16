using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ICollection<UserGetDto>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<ICollection<UserGetDto>> GetUsersPagitationAsync(int count, int side, CancellationToken cancellationToken);
        Task<ICollection<UserGetDto>> GetUsersByNameAsync(string name, CancellationToken cancellationToken);
        Task<ICollection<UserGetDto>> GetUsersByNamePagitationAsync(string name, int count, int side, CancellationToken cancellationToken);
        Task<UserGetDto> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserGetDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task<int?> DeleteUserByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Allows the user to update only themselves
        /// </summary>
        /// <param name="userUpdateDto">Object that describe how new user must look. Fields equal to zero 
        /// will not be subject to change</param>
        /// <param name="cancellationToken"></param>
        /// <param name="userId">Id of the user who run the request</param>
        /// <returns></returns>
        Task<UserGetDto> UpdateUserForUserAsync(UserUpdateDto userUpdateDto, int userId, CancellationToken cancellationToken);
        /// <summary>
        /// Allows the admin to update any user
        /// </summary>
        /// <param name="userUpdateDto">Object that describe how new user must look. Fields equal to zero 
        /// will not be subject to change</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserGetDto> UpdateUserForAdminAsync(UserUpdateDto userUpdateDto, CancellationToken cancellationToken);
        Task<int?> CreateUserAsync(UserPostDto userPostDto, CancellationToken cancellationToken);
        Task<RefreshTokenEntity> LoginWithPasswordAsync(UserLoginDto loginDto, CancellationToken cancellationToken);
        Task<RefreshTokenEntity> LoginWithRecoveryTokenAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken);
        Task<string> RefreshAsync(string refreshToken, CancellationToken cancellationToken);

        /// <summary>
        /// Returns false if a user with this email address was not found. 
        /// Returns true and creates a token if the user was found.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> CreateRecoveryTokenAsync(string email, CancellationToken cancellationToken);

    }
}
