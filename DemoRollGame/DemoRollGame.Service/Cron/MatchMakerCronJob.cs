using DemoRollGame.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoRollGame.Service.Cron
{

    public class MatchMakerCronJob : ICronJob
    {
        public string GetScheduleFormat(IServiceScope serviceScope)
        {
            var conf = serviceScope.ServiceProvider
                .GetRequiredService<IOptions<MatchMakerConfiguration>>()
                .Value;
            return $"*/{conf.DailyRunMinute} * * * * *"; 
        }
        public DateTime NextRun { get; set; }
        public CrontabSchedule Schedule { get; set; }

        public Task ExecuteAsync(CancellationToken cancellationToken, IServiceScope serviceScope)
        {
            //Get initial required services
            var conf = serviceScope.ServiceProvider
                .GetRequiredService<IOptions<MatchMakerConfiguration>>()
                .Value;
            if (!conf.Enable)
                return Task.CompletedTask;

            MatchService matchService= serviceScope.ServiceProvider
                .GetRequiredService<MatchService>();
            PlayService playService = serviceScope.ServiceProvider
               .GetRequiredService<PlayService>();
            var currentMatch = matchService.GetCurrentMatch();
            if(currentMatch==null)
            {
                //create new match
                matchService.CreateMatch();
            }
            else
            {
                if(currentMatch.ExpiresAt<DateTime.Now)
                {
                    //complate Match
                    playService.DecideToWinner();
                    matchService.ComplateMatch(currentMatch.Id);
                    matchService.CreateMatch();
                }
            }


            return Task.CompletedTask;
        }
        
    }
}
