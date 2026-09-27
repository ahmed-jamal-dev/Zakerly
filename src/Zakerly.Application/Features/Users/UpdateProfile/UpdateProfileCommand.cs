using MediatR;

namespace Zakerly.Application.Features.Users.UpdateProfile;

public sealed record UpdateProfileCommand(
    string FullName,
    string Email
) : IRequest<UpdateProfileResponse>;
