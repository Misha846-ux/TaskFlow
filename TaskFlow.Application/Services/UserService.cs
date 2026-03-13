using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces.Helpers;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;

namespace TaskFlow.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashHelper _hashHelper;
        public UserService(IUserRepository userRepository, IHashHelper hashHelper)
        {
            _userRepository = userRepository;
            _hashHelper = hashHelper;
        }

        public Task DeleteUserByIdForAdminAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserByIdForUserAsync(int deleteId, int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserGetDto>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
