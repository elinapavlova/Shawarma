using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Configurations;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Models.Tokens;
using Models.User;
using Services.Contracts;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly TokenOptions _tokenOptions;
        private readonly SigningConfiguration _signingConfiguration;

        public JwtService
        (
            IOptions<TokenOptions> options, 
            SigningConfiguration signingConfiguration
        )
        {
            _tokenOptions = options.Value;
            _signingConfiguration = signingConfiguration;
        }
        
        public AccessToken CreateAccessToken(UserCredentialsDto user)
        {
            var refreshToken = BuildRefreshToken();
            _refreshTokens.Add(refreshToken);
            var accessToken = BuildAccessToken(user, refreshToken);
            _refreshTokens.Add(refreshToken);

            return accessToken;
        }

        public RefreshToken TakeRefreshToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var refreshToken = _refreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken != null)
                _refreshTokens.Remove(refreshToken);

            return refreshToken;
        }

        private RefreshToken BuildRefreshToken()
        {
            var refreshToken = new RefreshToken
            (
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddSeconds(_tokenOptions.RefreshTokenExpiration).Ticks
            );

            return refreshToken;
        }

        private AccessToken BuildAccessToken(UserCredentialsDto user, RefreshToken refreshToken)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenExpiration);
            var claims = GetClaims(user);

            if (claims == null)
                return null;
            
            var securityToken = new JwtSecurityToken
            (
                _tokenOptions.Issuer,
                _tokenOptions.Audience,
                claims,
                expires : accessTokenExpiration,
                notBefore : DateTime.UtcNow,
                signingCredentials : _signingConfiguration.SigningCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(securityToken);

            return new AccessToken(accessToken, accessTokenExpiration.Ticks, refreshToken);
        }

        private static IEnumerable<Claim> GetClaims(UserCredentialsDto user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, user.Email),
            };
            return claims;
        }
    }
}