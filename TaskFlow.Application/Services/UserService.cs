using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Helpers;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashHelper _hashHelper;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper, IHashHelper hashHelper)
        {
            _userRepository = userRepository;
            _hashHelper = hashHelper;
            _mapper = mapper;
        }

        public Task<bool> CreateRecoveryTokenAsync(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int?> CreateUserAsync(UserPostDto userPostDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> DeleteUserByIdForAdminAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _userRepository.DeleteUserByIdAsync(id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: DeleteUserByIdForAdminAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with DeleteUserByIdForAdminAsync");
            }
        }

        public async Task<int?> DeleteUserByIdForUserAsync(int deleteId, int userId, CancellationToken cancellationToken)
        {
            try
            {
                if(userId != deleteId)
                {
                    throw new Exception("No access");
                }
                return await _userRepository.DeleteUserByIdAsync(deleteId, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: DeleteUserByIdForUserAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with DeleteUserByIdForUserAsync");
            }
        }

        public async Task<ICollection<UserGetDto>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                ICollection<UserEntity> users = await _userRepository.GetAllUsersAsync(cancellationToken);
                return _mapper.Map<ICollection<UserGetDto>>(users);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: GetAllUsersAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with GetAllUsersAsync");
            }
        }

        public Task<UserGetDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserGetDto> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserGetDto>> GetUsersByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserGetDto>> GetUsersByNamePagitationAsync(string name, int count, int side, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserGetDto>> GetUsersPagitationAsync(int count, int side, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenEntity> LoginWithPasswordAsync(UserLoginDto loginDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenEntity> LoginWithRecoveryTokenAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserGetDto> UpdateUserForAdminAsync(UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserGetDto> UpdateUserForUserAsync(UserUpdateDto userUpdateDto, int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
