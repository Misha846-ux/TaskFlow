using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<int?> AddUserAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public Task<int?> DeleteUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserEntity>?> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserEntity>?> GetUsersByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserEntity>?> GetUsersByNamePaginationAsync(string name, int count, int side)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserEntity>?> GetUsersPaginationAsync(int count, int side)
        {
            throw new NotImplementedException();
        }

        public Task<int?> UpdateUserByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
