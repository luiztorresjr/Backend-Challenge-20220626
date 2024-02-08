using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Cron
{
    [DisallowConcurrentExecution]
    public class LogBackgroundService : IJob
    {
        private readonly ILogger<LogBackgroundService> _logger;

        public LogBackgroundService(ILogger<LogBackgroundService> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{UtcNow}", DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}
