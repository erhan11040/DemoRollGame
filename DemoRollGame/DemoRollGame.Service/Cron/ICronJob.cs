using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoRollGame.Service.Cron
{
    /// <summary>
    /// Interface for defining scheduled tasks for the CronService
    /// </summary>
    public interface ICronJob
    {
        /// <summary>
        /// Get a six-part schedule format of this job, using service provider
        /// </summary>
        /// <returns>A a six-part schedule format (including seconds)</returns>
        string GetScheduleFormat(IServiceScope serviceScope);
        /// <summary>
        /// The next time this job will be executed (set by CronService)
        /// </summary>
        DateTime NextRun { get; set; }
        /// <summary>
        /// Crontab schedule of this object (set by CronService)
        /// </summary>
        CrontabSchedule Schedule { get; set; }
        /// <summary>
        /// This method will be called by cron executor on every schedule occurences
        Task ExecuteAsync(CancellationToken cancellationToken, IServiceScope serviceScope);
    }
}
