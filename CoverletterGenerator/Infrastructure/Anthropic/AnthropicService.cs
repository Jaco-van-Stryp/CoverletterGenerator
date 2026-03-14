using Anthropic;
using Anthropic.Models.Messages;
using Microsoft.Extensions.Options;

namespace CoverletterGenerator.Infrastructure.Anthropic
{
    public class AnthropicService : IAnthropicService
    {
        private readonly AnthropicClient _client;
        private readonly AnthropicOptions _options;

        public AnthropicService(IOptions<AnthropicOptions> options)
        {
            _options = options.Value;
            _client = new AnthropicClient { ApiKey = _options.ApiKey, BaseUrl = _options.BaseUrl };
        }

        public async Task<AiResponse> GenerateAIResponse(string prompt)
        {
            var message = await _client.Messages.Create(
                new MessageCreateParams
                {
                    Model = _options.Model,
                    MaxTokens = 8096,
                    Messages = [new() { Role = Role.User, Content = prompt }],
                }
            );

            return ToAiResponse(message);
        }

        public async Task<AiResponse> GenerateAIResponse(string systemPrompt, string prompt)
        {
            var message = await _client.Messages.Create(
                new MessageCreateParams
                {
                    Model = _options.Model,
                    MaxTokens = 8096,
                    System = systemPrompt,
                    Messages = [new() { Role = Role.User, Content = prompt }],
                }
            );

            return ToAiResponse(message);
        }

        private AiResponse ToAiResponse(global::Anthropic.Models.Messages.Message message)
        {
            var lastContent = message.Content.Last();
            if (!lastContent.TryPickText(out var textBlock))
                throw new InvalidOperationException("No text block in response.");

            return new AiResponse(
                Text: textBlock.Text,
                Model: _options.Model,
                InputTokens: message.Usage.InputTokens,
                OutputTokens: message.Usage.OutputTokens,
                CacheCreationInputTokens: message.Usage.CacheCreationInputTokens ?? 0,
                CacheReadInputTokens: message.Usage.CacheReadInputTokens ?? 0
            );
        }
    }
}
