using CoverletterGenerator.Infrastructure.ExceptionHandling;

namespace CoverletterGenerator.Features.GenerateCoverLetter
{
    public class CVNotFoundException()
        : ApiException("CV not found", StatusCodes.Status404NotFound);
}
