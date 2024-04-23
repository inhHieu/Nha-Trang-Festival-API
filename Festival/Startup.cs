
using Microsoft.Extensions.DependencyInjection;
using System;
using Festival.Controllers;
using Quartz.Simpl;
using Quartz;
using Quartz.Impl;

namespace Festival
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            // Add the SendSubscriptionEmailsJob class
            services.AddSingleton<SendSubscriptionEmailsJob>();

            // Add the Quartz scheduler
            //services.AddSingleton<IScheduler>(provider =>
            //{
            //    var factory = new StdSchedulerFactory();
            //    var scheduler = factory.GetScheduler().Result;
            //    scheduler.JobFactory = new JobFactory(provider);
            //    return scheduler;
            //});
            services.AddQuartz(q =>
            {
                //q.UseMicrosoftDependencyInjectionScopedJobFactory();
                //var jobKey = new JobKey("SendSubscriptionEmailsJob");
                //q.AddJob<SendSubscriptionEmailsJob>(opts => opts.WithIdentity(jobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(jobKey)
                //    .WithIdentity("SendSubscriptionEmailsJob-trigger")
                //    //This Cron interval can be described as "run every minute" (when second is zero)
                //    .WithCronSchedule("0 * * ? * *")
                //);
                // base Quartz scheduler, job and trigger configuration
            });
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();
        }
    }
}