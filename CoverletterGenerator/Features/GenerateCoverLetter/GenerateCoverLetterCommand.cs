using MediatR;

namespace CoverletterGenerator.Features.GenerateCoverLetter
{
    public readonly record struct GenerateCoverLetterCommand(string SeekJobUrl, Guid CVId, Guid UserId)
        : IRequest<GenerateCoverLetterResult>;
}
