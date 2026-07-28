using MediatR;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Enums;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Domain.Interfaces.Security;

namespace Zakerly.Application.Features.Authentication.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new Exception("Email already exists.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        
        var user = new User(
            request.FullName,
            request.Email,
            passwordHash,
            UserRole.Student);

        await _userRepository.AddAsync(user, cancellationToken);

        return new RegisterResponse(
            user.Id,
            user.FullName,
            user.Email);
    }
}