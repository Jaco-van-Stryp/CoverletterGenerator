using CoverletterGenerator.Infrastructure.Anthropic;

namespace CoverletterGenerator.Infrastructure.DependencyInjection
{
    public static class AnthropicServiceExtensions
    {
        public static IServiceCollection AddAnthropicService(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<AnthropicOptions>(configuration.GetSection("Anthropic"));
            services.AddScoped<IAnthropicService, AnthropicService>();
            return services;
        }
    }
}
