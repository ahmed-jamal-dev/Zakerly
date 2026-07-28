using MediatR;

namespace Zakerly.Application.Features.Authentication.Register;

public sealed record RegisterCommand(
    string FullName,
    string Email,
    string Password
) : IRequest<RegisterResponse>;