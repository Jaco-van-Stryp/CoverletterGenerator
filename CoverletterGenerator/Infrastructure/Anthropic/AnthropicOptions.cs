namespace CoverletterGenerator.Infrastructure.Anthropic
{
    public class AnthropicOptions
    {
        public required string ApiKey { get; set; }
        public required string Model { get; set; } = "claude-haiku-4-5";
        public required string BaseUrl { get; set; } = "https://api.anthropic.com";
    }
}
