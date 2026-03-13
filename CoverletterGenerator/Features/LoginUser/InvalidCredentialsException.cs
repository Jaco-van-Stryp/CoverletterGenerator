using CoverletterGenerator.Infrastructure.ExceptionHandling;

namespace CoverletterGenerator.Features.LoginUser
{
    public class InvalidCredentialsException()
        : ApiException("Invalid email or password.", StatusCodes.Status401Unauthorized);
}
