using Vega.UserService.Contracts;

namespace Vega.UserService.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default);
    Task<AuthResponse?> LoginAsync(LoginRequest req, CancellationToken ct = default);
}
