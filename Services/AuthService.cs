using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Result;
using Infrastructure.Validate;
using Models.Error;
using Models.Tokens;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        
        public AuthService
        (
            IUserService userService,
            IHttpClientFactory clientFactory,
            IJwtService jwtService,
            IMapper mapper
        )
        {
            _userService = userService;
            _clientFactory = clientFactory;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        
        
        public async Task<ResultContainer<AccessTokenDto>> RefreshTokenAsync(string refreshToken, string userEmail)
        {
            var result = new ResultContainer<AccessTokenDto>();
            var token = _jwtService.TakeRefreshToken(refreshToken);
            var userDto = await _userService.GetByEmail(userEmail);
            
            if (token == null || token.IsExpired() || userDto.Data == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = _mapper.Map <UserCredentialsDto>(userDto);
            result = _mapper.Map<ResultContainer<AccessTokenDto>>(_jwtService.CreateAccessToken(user));
            return result;
        }

        public async Task<ResultContainer<AccessTokenDto>> Login(UserCredentialsDto data)
        {
            var user = await _userService.GetByEmail(data.Email);
            var result = new ResultContainer<AccessTokenDto>();

            if (user.Data == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            result = await CreateAccessTokenAsync(data.Email, data.Password);
            return result;
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
            using var client = _clientFactory.CreateClient("Dadata");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", address),
            });
            
            var response = await client.PostAsync("Validate", content);

            var res = await response.Content.ReadAsStringAsync();
            return res;
        }
        
        private async Task<ResultContainer<AccessTokenDto>> CreateAccessTokenAsync(string email, string password)
        {
            var userDto = await _userService.GetByEmail(email);
            var result = new ResultContainer<AccessTokenDto>();
            
            if (userDto.Data == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var isEqualPasswords = string.Equals(password, userDto.Data.Password);
            
            if (!isEqualPasswords)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var user = _mapper.Map<UserCredentialsDto>(userDto);
            result = _mapper.Map<ResultContainer<AccessTokenDto>>(_jwtService.CreateAccessToken(user));
            return result;
        }
    }
}