using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Result;
using Infrastructure.Validate;
using Microsoft.Extensions.Configuration;
using Models.Error;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly Uri _baseAddress;
        public AuthService
        (
            IUserService userService,
            IJwtService jwtService,
            IConfiguration configuration
        )
        {
            _userService = userService;
            _jwtService = jwtService;
            _baseAddress = new Uri(configuration["BaseAddress:DadataUri"]);
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
            var res = await ValidateAddress(dto.Address);
            var isValidEmail = Validator.EmailIsValid(dto.Email);
            
            if (user.Data != null || res == "" || !isValidEmail)
            {
                user.ErrorType = ErrorType.BadRequest;
                return user;
            }

            var result = await _userService.Create(dto);
            return result;
        }

        private async Task<string> ValidateAddress(string address)
        {
            using var client = new HttpClient();
            client.BaseAddress = _baseAddress;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", address),
            });
            
            var response = await client.PostAsync("Validate", content);

            var res = await response.Content.ReadAsStringAsync();
            return res;
        }
    }
}