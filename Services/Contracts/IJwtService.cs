using System.IdentityModel.Tokens.Jwt;

namespace Services.Contracts
{
    public interface IJwtService
    {
        string Generate(int id, int idRole);
        JwtSecurityToken Verify(string jwt);
    }
}