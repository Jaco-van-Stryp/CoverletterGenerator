using CoverletterGenerator.Infrastructure.ExceptionHandling;

namespace CoverletterGenerator.Features.GenerateCoverLetter
{
    public class CVAccessDeniedException()
        : ApiException("You do not have access to this CV.", StatusCodes.Status403Forbidden);
}
