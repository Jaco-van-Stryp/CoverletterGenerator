using CoverletterGenerator.Infrastructure.Anthropic;
using CoverletterGenerator.Infrastructure.PDF;
using CoverletterGenerator.Infrastructure.Scraper;

namespace CoverletterGenerator.Infrastructure.DependencyInjection
{
    public static class ScraperServiceExtensions
    {
        public static IServiceCollection AddPlaywrightServices(this IServiceCollection services)
        {
            Microsoft.Playwright.Program.Main(["install", "chromium"]);

            services.AddScoped<IScraperService, ScraperService>();
            services.AddScoped<IPdfService, PdfService>();
            return services;
        }
    }
}
