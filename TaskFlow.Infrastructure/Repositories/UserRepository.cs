using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Helpers;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskFlowDbContext _context;
        private readonly IHashHelper _hashHelper;
        public UserRepository(TaskFlowDbContext context, IHashHelper hashHelper)
        {
            _context = context;
            _hashHelper = hashHelper;
        }
        public async Task<int?> AddUserAsync(UserEntity user, string password, CancellationToken cancellationToken)
        {
            try
            {
                user.PasswordHash = _hashHelper.Hash(password);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync(cancellationToken);
                return user.Id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: AddUserAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with AddUserAsync");
            }
        }

        public async Task<int?> DeleteUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _context.Users.Remove(new UserEntity { Id = id });
                await _context.SaveChangesAsync(cancellationToken);
                return id;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: DeleteUserByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with DeleteUserByIdAsync");
            }
        }

        public async Task<ICollection<UserEntity>?> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Projects)
                    .Include(u => u.Changes)
                    .Include(u => u.RefreshTokens)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: GetAllUsersAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with GetAllUsersAsync");
            }
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Projects)
                    .Include(u => u.Changes)
                    .Include(u => u.RefreshTokens)
                    .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: GetUserByEmailAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with GetUserByEmailAsync");
            }
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Projects)
                    .Include(u => u.Changes)
                    .Include(u => u.RefreshTokens)
                    .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: GetUserByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with GetUserByIdAsync");
            }
        }

        public async Task<ICollection<UserEntity>?> GetUsersByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Projects)
                    .Include(u => u.Changes)
                    .Include(u => u.RefreshTokens)
                    .Where(u => u.UserName.Contains(name))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: GetUsersByNameAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with GetUsersByNameAsync");
            }
        }

        public async Task<ICollection<UserEntity>?> GetUsersByNamePaginationAsync(string name, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if(side < 1)
                {
                    side = 1;
                }
                return await _context.Users
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Projects)
                    .Include(u => u.Changes)
                    .Include(u => u.RefreshTokens)
                    .OrderBy(u => u.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .Where(u => u.UserName.Contains(name))
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: GetUsersByNamePaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with GetUsersByNamePaginationAsync");
            }
        }

        public async Task<ICollection<UserEntity>?> GetUsersPaginationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                if (side < 1)
                {
                    side = 1;
                }
                return await _context.Users
                    .Include(u => u.Tasks)
                    .Include(u => u.Companies)
                    .Include(u => u.Projects)
                    .Include(u => u.Changes)
                    .Include(u => u.RefreshTokens)
                    .OrderBy(u => u.Id)
                    .Skip((side - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: GetUsersPaginationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with GetUsersPaginationAsync");
            }
        }

        public async Task<UserEntity> UpdateAsync(UserEntity newUser, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = await GetUserByIdAsync(newUser.Id, cancellationToken);
                if (user == null)
                {
                    throw new Exception();
                }
                PropertyInfo[] properties = typeof(UserEntity).GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    if(prop.GetValue(newUser) != null)
                    {
                        prop.SetValue(user, newUser);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                return user;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Repository: UpdateAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Repository: Problem with UpdateAsync");
            }
        }
    }
}
