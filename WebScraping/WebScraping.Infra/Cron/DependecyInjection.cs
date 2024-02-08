using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Scraping;

namespace WebScraping.Infra.Cron
{
    public static class DependecyInjection
    {
        public static void AddInfraestrutura(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(LogBackgroundService));
                options
                    .AddJob<LogBackgroundService>(jobKey)
                    .AddTrigger(trigger => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever()         
                ));

                // roda o processo de web scraping todo dias as meia noite
                var jobGetWebScaping = JobKey.Create(nameof(CronTaskScrapingService));
                options
                   .AddJob<CronTaskScrapingService>(jobGetWebScaping)
                   .AddTrigger(trigger => trigger.ForJob(jobGetWebScaping)
                       .WithCronSchedule("0 0 0 1/1 * ? *"));

            });
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete  = true;
            });
        }
    }
}
