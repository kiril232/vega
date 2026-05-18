using System.ComponentModel.DataAnnotations;

namespace Vega.UserService.Contracts;

public record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password,
    [Required, MaxLength(256)] string FullName);

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record AuthResponse(string Token, UserResponse User);

public record UserResponse(Guid Id, string Email, string FullName);
