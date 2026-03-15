using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHashHelper _hashHelper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, IHashHelper hashHelper
            , ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _hashHelper = hashHelper;
            _mapper = mapper;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public Task<bool> CreateRecoveryTokenAsync(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> CreateUserAsync(UserPostDto userPostDto, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = _mapper.Map<UserEntity>(userPostDto);
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

        public async Task<UserGetDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
                return _mapper.Map<UserGetDto>(user);
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
                UserEntity user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
                return _mapper.Map<UserGetDto>(user);
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
                ICollection<UserEntity> users = await _userRepository.GetUsersByNameAsync(name, cancellationToken);
                return _mapper.Map<ICollection<UserGetDto>>(users);
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
                ICollection<UserEntity> users = await _userRepository.GetUsersByNamePaginationAsync(name, count, side, cancellationToken);
                return _mapper.Map<ICollection<UserGetDto>>(users);
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
                ICollection<UserEntity> users = await _userRepository.GetUsersPaginationAsync(count, side, cancellationToken);
                return _mapper.Map<ICollection<UserGetDto>>(users);
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

                Console.WriteLine("Метод LoginWithPasswordAsync не закончен");
                throw new Exception();
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
                else if (user.RecoveryTokenLifeTime == DateTime.Now)
                {
                    throw new UnauthorizedAccessException();
                }
                else if (!_hashHelper.IsValidPassword(userLoginDto.Password, user.RecoveryTokenHash))
                {
                    throw new UnauthorizedAccessException();
                }

                Console.WriteLine("Метод LoginWithRecoveryTokenAsync не закончен");
                throw new Exception();
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

        public Task<string> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserGetDto> UpdateUserForAdminAsync(UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
        {
            try
            {
                UserEntity newUser = _mapper.Map<UserEntity>(userUpdateDto);
                UserEntity user = await _userRepository.UpdateAsync(newUser, cancellationToken);
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
                if(userId != userUpdateDto.Id)
                {
                    throw new Exception();
                }
                UserEntity newUser = _mapper.Map<UserEntity>(userUpdateDto);
                UserEntity user = await _userRepository.UpdateAsync(newUser, cancellationToken);
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
