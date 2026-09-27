namespace Zakerly.Application.Features.Users.UpdateProfile;

public sealed record UpdateProfileResponse(
    Guid UserId,
    string FullName,
    string Email
);
