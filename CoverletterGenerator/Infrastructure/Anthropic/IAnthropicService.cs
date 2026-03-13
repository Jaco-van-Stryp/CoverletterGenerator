namespace CoverletterGenerator.Infrastructure.Anthropic
{
    public interface IAnthropicService
    {
        Task<string> GenerateAIResponse(string prompt);
        Task<string> GenerateAIResponse(string systemPrompt, string prompt);
    }
}
