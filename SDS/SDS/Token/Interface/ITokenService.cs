using SDS.Domain.Entities;

namespace SDS.Token.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
