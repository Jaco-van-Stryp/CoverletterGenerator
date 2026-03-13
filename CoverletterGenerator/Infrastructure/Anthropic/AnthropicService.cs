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

        public async Task<string> GenerateAIResponse(string prompt)
        {
            var message = await _client.Messages.Create(
                new MessageCreateParams
                {
                    Model = _options.Model,
                    MaxTokens = 8096,
                    Messages = [new() { Role = Role.User, Content = prompt }],
                }
            );

            var lastContent = message.Content.Last();
            if (lastContent.TryPickText(out var textBlock))
                return textBlock.Text;

            throw new InvalidOperationException("No text block in response.");
        }

        public async Task<string> GenerateAIResponse(string systemPrompt, string prompt)
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

            var lastContent = message.Content.Last();
            if (lastContent.TryPickText(out var textBlock))
                return textBlock.Text;

            throw new InvalidOperationException("No text block in response.");
        }
    }
}
