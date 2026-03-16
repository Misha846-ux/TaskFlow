using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskFlow.Application.DTOs.TaskDTOs;
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
        private readonly ICachingService _cacheService;
        private readonly IJwtService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, IHashHelper hashHelper
            , IJwtService tokenService, ICachingService cacheService) //IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _hashHelper = hashHelper;
            _mapper = mapper;
            _tokenService = tokenService;
            //_refreshTokenRepository = refreshTokenRepository;
            _cacheService = cacheService;

        }

        public async Task<string> CreateRecoveryTokenAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
                if (user == null)
                {
                    throw new Exception();
                }
                
                string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                user.RecoveryTokenHash = _hashHelper.Hash(token);
                user.RecoveryTokenLifeTime = DateTime.UtcNow.AddMinutes(5);
                await _userRepository.SaveChages(cancellationToken);

                return token;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: CreateRecoveryTokenAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with CreateRecoveryTokenAsync");
            }
        }

        public async Task<int?> CreateUserAsync(UserPostDto userPostDto, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = _mapper.Map<UserEntity>(userPostDto);
                await _cacheService.RemoveAsync("Users");
                await _cacheService.RemoveAsync($"Users:email:{user.Email.ToLower()}");
                await _cacheService.RemoveAsync($"Users:id:{user.Id}");
                await _cacheService.RemoveAsync($"Users:name:{user.UserName}");
                user.Email = user.Email.Trim();
                return await _userRepository.AddUserAsync(user, userPostDto.Password, cancellationToken);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: CreateUserAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with CreateUserAsync");
            }
        }

        public async Task<int?> DeleteUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

                if (user == null)
                    return null;
                await _cacheService.RemoveAsync("Users");
                await _cacheService.RemoveAsync($"Users:email:{user.Email.ToLower()}");
                await _cacheService.RemoveAsync($"Users:id:{user.Id}");
                await _cacheService.RemoveAsync($"Users:name:{user.UserName}");
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

        public async Task<ICollection<UserGetDto>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<UserGetDto>>("Users");
                if (cache == null)
                {
                    ICollection<UserEntity> users = await _userRepository.GetAllUsersAsync(cancellationToken);
                    cache = _mapper.Map<ICollection<UserGetDto>>(users);
                    await _cacheService.SetAsync("Users", cache, null);
                }
                return cache;
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

        public async Task<UserGetDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<UserGetDto>($"Users:email:{email.ToLower()}");
                if (cache == null)
                {
                    UserEntity user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
                    cache = _mapper.Map<UserGetDto>(user);
                    await _cacheService.SetAsync($"Users:email:{email.ToLower()}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: GetUserByEmailAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with GetUserByEmailAsync");
            }
        }

        public async Task<UserGetDto> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<UserGetDto>($"Users:id:{id}");
                if (cache == null)
                {
                    UserEntity user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
                    cache = _mapper.Map<UserGetDto>(user);
                    await _cacheService.SetAsync($"Users:id:{id}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: GetUserByIdAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with GetUserByIdAsync");
            }
        }

        public async Task<ICollection<UserGetDto>> GetUsersByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<UserGetDto>>($"Users:name:{name}");
                if (cache == null)
                {
                    ICollection<UserEntity> users = await _userRepository.GetUsersByNameAsync(name, cancellationToken);
                    cache = _mapper.Map<ICollection<UserGetDto>>(users);
                    await _cacheService.SetAsync($"Users:name:{name}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: GetUsersByNameAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with GetUsersByNameAsync");
            }
        }

        public async Task<ICollection<UserGetDto>> GetUsersByNamePagitationAsync(string name, int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<UserGetDto>>($"Users:name:pagination:{name}:{count}:{side}");
                if (cache == null)
                {
                    ICollection<UserEntity> users = await _userRepository.GetUsersByNamePaginationAsync(name, count, side, cancellationToken);
                    cache = _mapper.Map<ICollection<UserGetDto>>(users);
                    await _cacheService.SetAsync($"Users:name:pagination:{name}:{count}:{side}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: GetUsersByNamePagitationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with GetUsersByNamePagitationAsync");
            }
        }

        public async Task<ICollection<UserGetDto>> GetUsersPagitationAsync(int count, int side, CancellationToken cancellationToken)
        {
            try
            {
                var cache = await _cacheService.GetAsync<ICollection<UserGetDto>>($"Users:pagination:{count}:{side}");
                if (cache == null)
                {
                    ICollection<UserEntity> users = await _userRepository.GetUsersPaginationAsync(count, side, cancellationToken);
                    cache = _mapper.Map<ICollection<UserGetDto>>(users);
                    await _cacheService.SetAsync($"Users:pagination:{count}:{side}", cache, null);
                }
                return cache;
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: GetUsersPagitationAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with GetUsersPagitationAsync");
            }
        }

        public async Task<RefreshTokenEntity> LoginWithPasswordAsync(UserLoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = await _userRepository.GetUserByEmailAsync(loginDto.Email.Trim(), cancellationToken);
                if(user == null)
                {
                    throw new UnauthorizedAccessException();
                }
                else if(!_hashHelper.IsValidPassword(loginDto.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException();
                }

                return await _tokenService.GenerateRefreshTokenAsync(user.Id);
            }
            catch (UnauthorizedAccessException uex)
            {
                throw new UnauthorizedAccessException("Incorrect email or password");
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: LoginWithPasswordAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with LoginWithPasswordAsync");
            }
        }

        public async Task<RefreshTokenEntity> LoginWithRecoveryTokenAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email.Trim(), cancellationToken);
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }
                else if (user.RecoveryTokenLifeTime >= DateTime.Now)
                {
                    throw new UnauthorizedAccessException();
                }
                else if (!_hashHelper.IsValidPassword(userLoginDto.Password, user.RecoveryTokenHash))
                {
                    throw new UnauthorizedAccessException();
                }

                user.RecoveryTokenHash = null;
                user.RecoveryTokenLifeTime = null;

                return await _tokenService.GenerateRefreshTokenAsync(user.Id);
            }
            catch (UnauthorizedAccessException uex)
            {
                throw new UnauthorizedAccessException("Invalid email or token, or token expired");
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: LoginWithRecoveryTokenAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with LoginWithRecoveryTokenAsync");
            }
        }

        public async Task<string> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                RefreshTokenEntity token = await _tokenService.GetRefreshTokenAsync(refreshToken);
                if(token == null)
                {
                    throw new Exception("Token not found");
                }
                else if(token.Expires >= DateTime.Now)
                {
                    throw new Exception("The token's lifetime has expired");
                }
                UserEntity user = await _userRepository.GetUserByIdAsync(token.UserId.Value, cancellationToken);
                return _tokenService.GenerateAccessToken(user);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: LoginWithRecoveryTokenAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with LoginWithRecoveryTokenAsync");
            }
        }

        public async Task<UserGetDto> UpdateUserForAdminAsync(UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity newUser = _mapper.Map<UserEntity>(userUpdateDto);
                UserEntity user = await _userRepository.UpdateAsync(newUser, cancellationToken);
                await _cacheService.RemoveAsync("Users");
                await _cacheService.RemoveAsync($"Users:email:{user.Email.ToLower()}");
                await _cacheService.RemoveAsync($"Users:id:{user.Id}");
                await _cacheService.RemoveAsync($"Users:name:{user.UserName}");
                return _mapper.Map<UserGetDto>(user);
                 
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: UpdateUserForAdminAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with UpdateUserForAdminAsync");
            }
        }

        public async Task<UserGetDto> UpdateUserForUserAsync(UserUpdateDto userUpdateDto, int userId, CancellationToken cancellationToken)
        {
            try
            {
                if (userId != userUpdateDto.Id)
                {
                    throw new Exception();
                }
                UserEntity newUser = _mapper.Map<UserEntity>(userUpdateDto);
                UserEntity user = await _userRepository.UpdateAsync(newUser, cancellationToken);
                await _cacheService.RemoveAsync("Users");
                await _cacheService.RemoveAsync($"Users:email:{user.Email.ToLower()}");
                await _cacheService.RemoveAsync($"Users:id:{user.Id}");
                await _cacheService.RemoveAsync($"Users:name:{user.UserName}");
                return _mapper.Map<UserGetDto>(user);

            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("User Sevice: UpdateUserForUserAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("User Service: Problem with UpdateUserForUserAsync");
            }
        }
    }
}
