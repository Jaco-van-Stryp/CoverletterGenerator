using System;

namespace CoverletterGenerator.Infrastructure.Scraper;

public interface IScraperService
{
    Task<string> GetJobDescription(string url);
}
