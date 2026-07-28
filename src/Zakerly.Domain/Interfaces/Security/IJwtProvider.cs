using Zakerly.Domain.Entities;

namespace Zakerly.Domain.Interfaces.Security;

public interface IJwtProvider
{
    string Generate(User user);
}