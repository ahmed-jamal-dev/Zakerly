namespace Zakerly.Application.Features.Authentication.Register;

public sealed record RegisterResponse(
    Guid UserId,
    string FullName,
    string Email
);