using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ContractManagementSystem.Controllers
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<ContractExpiry>().Build();
            IJobDetail job2 = JobBuilder.Create<AlertsConfiguration>().Build();
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(12)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(2, 8))
            //      )
            //    .Build();
            int Hours = 6;
            int Minutes = 0;


            try
            {


                Minutes = Convert.ToInt32(WebConfigurationManager.AppSettings["ScheduleMinutes"]);
                Hours = Convert.ToInt32(WebConfigurationManager.AppSettings["ScheduleHours"]);
            }
            catch { }
            //int ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInHours(Hours)
                .WithIntervalInMinutes(Minutes)
                .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);

            ITrigger trigger2 = TriggerBuilder.Create()
           .WithDailyTimeIntervalSchedule
                 (s =>
                    s.WithIntervalInHours(24)
                   .OnEveryDay()
                   .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(15, 26))
                 )
               .Build();

            scheduler.ScheduleJob(job2, trigger2);

        }
    }
}