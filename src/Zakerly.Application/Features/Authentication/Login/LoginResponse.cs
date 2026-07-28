namespace Zakerly.Application.Features.Authentication.Login;

public record LoginResponse(
    Guid UserId,
    string FullName,
    string Email,
    string Token);