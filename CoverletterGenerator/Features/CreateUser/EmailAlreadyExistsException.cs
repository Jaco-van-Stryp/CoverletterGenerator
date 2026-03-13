using CoverletterGenerator.Infrastructure.ExceptionHandling;

namespace CoverletterGenerator.Features.CreateUser
{
    public class EmailAlreadyExistsException()
        : ApiException("An account with that email already exists.", StatusCodes.Status409Conflict);
}
