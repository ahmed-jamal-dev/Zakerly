using MediatR;
using Zakerly.Application.Common.Exceptions;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Domain.Interfaces.Security;

namespace Zakerly.Application.Features.Authentication.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);
        if (user is null)
            throw new InvalidCredentialsException();
        if (!_passwordHasher.Verify(
                request.Password,
                user.PasswordHash))
        {
            throw new InvalidCredentialsException();
            
        }

        var token = _jwtProvider.Generate(user);
        return new LoginResponse(
            user.Id,
            user.FullName,
            user.Email,
            token);
    }
}