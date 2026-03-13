using CoverletterGenerator.Entities;

namespace CoverletterGenerator.Infrastructure.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
