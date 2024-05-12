using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        [HttpPost]
        [Route("CreateBackgrounJob")]
        public IActionResult CreateBackgrounJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Background job triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("ScheduleBackgrounJob")]
        public IActionResult ScheduleBackgrounJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(8);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);

            BackgroundJob.Schedule(() => Console.WriteLine("Schedule Background job triggered"),dateTimeOffset);
            return Ok();
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        // Continuation Job run immediately after another job has completed
        public IActionResult CreateContinuationJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(8);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);

           var jobId =  BackgroundJob.Schedule(() => Console.WriteLine("Schedule Background job 33 triggered"), dateTimeOffset);
           var job2Id =  BackgroundJob.ContinueJobWith(jobId,() => Console.WriteLine("Schedule Background job 1 triggered"));
           var job3Id =  BackgroundJob.ContinueJobWith(job2Id, () => Console.WriteLine("Schedule Background job 2 triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public IActionResult CreateRecurringJob()
        {
           // RecurringJob.AddOrUpdate("RecurringJob-1", () => Console.WriteLine("Recurring Job triggered"), Cron.Daily);
            RecurringJob.AddOrUpdate("RecurringJob-2", () => Console.WriteLine("Recurring RecurringJob-2 triggered"), "* * * * *");
            return Ok();
        }


        #region Example with Hangfire from old project i used it with OrderService Notification
        //var serviceProvider = builder.Services.BuildServiceProvider();
        //var recurringJobManager = serviceProvider.GetService<IRecurringJobManager>();
        //var orderService = serviceProvider.GetService<IOrderService>();

        //recurringJobManager.AddOrUpdate(
        //    "CheckIncompleteOrdersDesignForNotification", // Unique identifier for the job
        //    () => orderService.CheckIncompleteOrdersDesignForNotification(), // Method to execute
        //    Cron.Daily // Cron expression defining the schedule (daily)
        //);
        #endregion
    }
}