using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        User user,
        CancellationToken cancellationToken);
}
