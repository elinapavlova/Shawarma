using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;

        public JwtService(AppSettingsOptions appSettings)
        {
            _secretKey = appSettings.Secret;
        }
        
        public string Generate(int id, int idRole)
        {
            IEnumerable<Claim> claims = new List<Claim>
            {
                new ("idRole", idRole.ToString())
            };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            
            var header = new JwtHeader(credentials);
            
            var payload = new JwtPayload
                (id.ToString(), null, claims,null, DateTime.Today.AddDays(1)); // 1 day
            
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(_secretKey);
            
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out var validatedToken);

            return (JwtSecurityToken) validatedToken;
        }
    }
}