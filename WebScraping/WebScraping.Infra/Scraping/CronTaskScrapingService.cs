using Microsoft.Extensions.Logging;
using Quartz;

namespace WebScraping.Infra.Scraping
{
    [DisallowConcurrentExecution]
    public class CronTaskScrapingService : IJob
    {
        private readonly ILogger<CronTaskScrapingService> _logger;
        private readonly IWebScrapingService _scrapingService;
        public CronTaskScrapingService(ILogger<CronTaskScrapingService> logger, IWebScrapingService scrapingService)
        {
            _logger = logger;
            _scrapingService = scrapingService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{UtcNow}", DateTime.UtcNow);
            return _scrapingService.GetProductUsingScraping();
            
        }
    }
}
