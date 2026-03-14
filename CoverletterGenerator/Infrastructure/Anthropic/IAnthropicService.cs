namespace CoverletterGenerator.Infrastructure.Anthropic
{
    public record AiResponse(
        string Text,
        string Model,
        long InputTokens,
        long OutputTokens,
        long CacheCreationInputTokens,
        long CacheReadInputTokens
    );

    public interface IAnthropicService
    {
        Task<AiResponse> GenerateAIResponse(string prompt);
        Task<AiResponse> GenerateAIResponse(string systemPrompt, string prompt);
    }
}
