using Quartz;
using System;

namespace Infrastructure.Jobs
{
    public class JobConfig
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public JobConfig(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public void ConfigureAndStartScheduler()
        {
            IScheduler scheduler = _schedulerFactory.GetScheduler().Result;

            AppDomain.CurrentDomain.DomainUnload += (sender, args) =>
            {
                if (scheduler != null && !scheduler.IsShutdown)
                {
                    scheduler.Shutdown(waitForJobsToComplete: true);
                }
            };

            IJobDetail job = JobBuilder.Create<ValidateApprovedEnrollmentsJob>()
                .WithIdentity("ValidateApprovedEnrollmentsJob", "ValidateApprovedEnrollmentJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("ValidateApprovedEnrollmentsJobTrigger", "ValidateApprovedEnrollmentJob")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);

            scheduler.Start();
        }
    }
}
