using Microsoft.Playwright;

namespace CoverletterGenerator.Infrastructure.Scraper;

public class ScraperService : IScraperService
{
    public async Task<string> GetJobDescription(string url)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        await page.GotoAsync(url);
        await page.WaitForSelectorAsync("[data-automation='jobAdDetails']");

        return await page.Locator("[data-automation='jobAdDetails']").InnerTextAsync();
    }
}
