using DemoRollGame.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoRollGame.Service.Cron
{
    public class CronService : BackgroundService
    {
        #region fields
        private readonly IServiceProvider _services;
        private readonly int _runPeriodInMiliseconds;
        private readonly Queue<ICronJob> _jobQueue;
        #endregion

        #region ctor
        public CronService(IServiceProvider services,
            IOptions<CronServiceConfiguration> configuration)
        {
            _services = services;
            _runPeriodInMiliseconds = configuration.Value.RunPeriodInSeconds * 1000;
            _jobQueue = new Queue<ICronJob>();

            //Get all jobs from current assemby at app startup
            Assembly ass = Assembly.GetExecutingAssembly();
            foreach (TypeInfo typeInfo in ass.DefinedTypes)
            {
                if (typeInfo.ImplementedInterfaces.Contains(typeof(ICronJob)))
                {
                    var job = ass.CreateInstance(typeInfo.FullName) as ICronJob;
                    using var serviceScope = _services.CreateScope();
                    string schedule = job.GetScheduleFormat(serviceScope);
                    job.Schedule = CrontabSchedule
                        .Parse(schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
                    job.NextRun = job.Schedule.GetNextOccurrence(DateTime.UtcNow);
                    _jobQueue.Enqueue(job);
                }
            }
        }
        #endregion

        #region service methods
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Process(stoppingToken);
                await Task.Delay(_runPeriodInMiliseconds, stoppingToken);
            }
        }

        private async Task Process(CancellationToken stoppingToken)
        {
            if (_jobQueue.Count > 0)
            {
                ICronJob job = _jobQueue.Dequeue();
                using var scope = _services.CreateScope();
                var now = DateTime.UtcNow;
                if (now > job.NextRun)
                {
                    string jobName = job.GetType().Name;
                    try
                    {
                        await job.ExecuteAsync(stoppingToken, scope);
                    }
                    catch (Exception ex)
                    {
                    }
                    job.NextRun = job.Schedule.GetNextOccurrence(DateTime.UtcNow);
                }
                _jobQueue.Enqueue(job);
            }
        }
        #endregion
    }
}
