using MediatR;

namespace Zakerly.Application.Features.Authentication.Login;

public record LoginCommand(
    string Email,
    string Password)
    : IRequest<LoginResponse>;