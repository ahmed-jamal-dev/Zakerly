using MediatR;
using Zakerly.Application.Common.Interfaces;
using Zakerly.Domain.Entities;
using Zakerly.Domain.Interfaces.Repositories;

namespace Zakerly.Application.Features.Users.UpdateProfile;

public class UpdateProfileCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateProfileCommand, UpdateProfileResponse>
{
    public async Task<UpdateProfileResponse> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        // Get current user ID
        var userId = currentUserService.UserId;
        
        // Get the user from repository
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }
        
        // Check if email is being changed and if it's already taken by another user
        if (!user.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailExists = await userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (emailExists)
            {
                throw new InvalidOperationException($"Email '{request.Email}' is already in use");
            }
        }
        
        // Update user profile
        user.UpdateProfile(request.FullName, request.Email);
        
        // Save changes
        await userRepository.UpdateAsync(user, cancellationToken);
        
        // Return response
        return new UpdateProfileResponse(
            user.Id,
            user.FullName,
            user.Email);
    }
}
