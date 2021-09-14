﻿using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Error;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        
        public AuthService
        (
            IUserService userService,
            IJwtService jwtService
        )
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        
        public async Task<ResultContainer<UserResponseDto>> VerifyUser(string username, string password)
        {
            var getUserByEmail = await _userService.GetByEmail(username);

            if (getUserByEmail.Data == null)
            {
                getUserByEmail.ErrorType = ErrorType.NotFound;
                return getUserByEmail;
            }
            
            if (username == getUserByEmail.Data.Email && password == getUserByEmail.Data.Password)
                return getUserByEmail;

            var res = new ResultContainer<UserResponseDto>
            {
                ErrorType = ErrorType.BadRequest
            };
            return res;
        }
        
        public async Task<ResultContainer<UserResponseDto>> VerifyJwt(string jwt)
        {
            var user = new ResultContainer<UserResponseDto>();

            if (jwt == null)
            {
                user.ErrorType = ErrorType.Unauthorized;
                return user;
            }
            
            var token = _jwtService.Verify(jwt);
            
            if (token == null)
            {
                user.ErrorType = ErrorType.Unauthorized;
                return user;
            }
            
            var userId = int.Parse(token.Issuer);
            user = await _userService.GetById(userId);
            return user;
        }
        
        public async Task<ResultContainer<UserResponseDto>> Login(UserLoginDto dto)
        {
            var user = await VerifyUser(dto.Email, dto.Password);
            return user;
        }
        
        public async Task<ResultContainer<UserResponseDto>> Register(UserRequestDto dto)
        {
            var user = await _userService.GetByEmail(dto.Email);

            // Если пользователь существует
            if (user.Data != null)
            {
                user.ErrorType = ErrorType.BadRequest;
                return user;
            }

            var result = await _userService.Create(dto);
            
            return result;
        }
    }
}