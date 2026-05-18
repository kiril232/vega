using Vega.UserService.Domain;

namespace Vega.UserService.Services;

public interface ITokenService
{
    string CreateToken(User user);
}
