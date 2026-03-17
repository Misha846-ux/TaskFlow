using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ICollection<UserEntity>?> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<ICollection<UserEntity>?> GetUsersPaginationAsync(int count, int side, CancellationToken cancellationToken);
    Task<ICollection<UserEntity>?> GetUsersByNameAsync(string name, CancellationToken cancellationToken);
    Task<ICollection<UserEntity>?> GetUsersByNamePaginationAsync(string name, int count, int side, CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<int?> DeleteUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<UserEntity> UpdateAsync(UserEntity newUser, CancellationToken cancellationToken);
    Task<int?> AddUserAsync(UserEntity user, string password, CancellationToken cancellationToken);// Method for adding user to database
    Task SaveChages(CancellationToken cancellationToken);
}