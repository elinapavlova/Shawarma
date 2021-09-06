using System.IdentityModel.Tokens.Jwt;

namespace Services.Contracts
{
    public interface IJwtService
    {
        string Generate(int id);
        JwtSecurityToken Verify(string jwt);
    }
}