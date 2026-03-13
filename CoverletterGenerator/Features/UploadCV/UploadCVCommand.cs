using MediatR;

namespace CoverletterGenerator.Features.UploadCV
{
    public record UploadCVCommand(IFormFile File, Guid UserId)
        : IRequest<UploadCVResult>;
}
