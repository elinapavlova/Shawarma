using Models.Tokens;
using Models.User;

namespace Services.Contracts
{
    public interface IJwtService
    {
        AccessToken CreateAccessToken(UserCredentialsDto user);
        RefreshToken TakeRefreshToken(string token);

    }
}