using Vega.UserService.Contracts;
using Vega.UserService.Data;
using Vega.UserService.Domain;

namespace Vega.UserService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;

    public AuthService(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        var email = req.Email.Trim().ToLower();

        // refuse duplicate signups early so we don't hit a unique constraint violation
        var existing = await _users.FindByEmailAsync(email, ct);
        if (existing is not null)
            throw new InvalidOperationException("email already registered");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = req.FullName.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return new AuthResponse(_tokens.CreateToken(user), ToDto(user));
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var user = await _users.FindByEmailAsync(req.Email.Trim().ToLower(), ct);
        if (user is null) return null;
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash)) return null;

        return new AuthResponse(_tokens.CreateToken(user), ToDto(user));
    }

    private static UserResponse ToDto(User u) => new(u.Id, u.Email, u.FullName);
}
