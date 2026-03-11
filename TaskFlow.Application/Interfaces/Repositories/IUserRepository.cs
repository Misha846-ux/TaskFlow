using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ICollection<UserEntity>?> GetAllUsersAsync();
    Task<ICollection<UserEntity>?> GetUsersPaginationAsync(int count, int side);
    Task<ICollection<UserEntity>?> GetUsersByNamePaginationAsync(int count, int side, string name);
    Task<ICollection<UserEntity>?> GetUsersByNameAsync(string name);
    Task<UserEntity?> GetUserByIdAsync(int id);
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<int?> DeleteUserByIdAsync(int id);
    Task<int?> UpdateUserByIdAsync(int id, UserEntity user); 
    Task<int?> AddUserAsync(UserEntity user);// Method for adding user to database
}
