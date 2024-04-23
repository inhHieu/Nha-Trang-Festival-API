using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Festival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SubscriptionController(IConfiguration configuration)
        {
            _configuration = configuration;

            ScheduleSendSubscriptionEmailsJob().Wait();

        }

        //[HttpGet]
        //public async Task<IActionResult> SendSubscriptionEmails()
        //{
        //    try
        //    {
        //        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        //        List<Subscription> subscriptions = new List<Subscription>();

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand command = new SqlCommand("sp_sendMail", connection);
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@stmttype", "sendmail");

        //            connection.Open();

        //            SqlDataReader reader = await command.ExecuteReaderAsync();

        //            while (reader.Read())
        //            {
        //                Subscription subscription = new Subscription();
        //                subscription.Email = reader["Email"].ToString();
        //                subscription.EventName = reader["EventName"].ToString();
        //                subscription.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);

        //                subscriptions.Add(subscription);
        //            }

        //            reader.Close();
        //        }

        //        foreach (Subscription subscription in subscriptions)
        //        {
        //            await SendEmail(subscription.Email);

        //            if (DateTime.Now.Date == subscription.ExpirationDate.Date)
        //            {
        //                // Send an email to the subscriber
        //                await SendEmail(subscription.Email);
        //            }
        //            return Ok("Subscription emails will be sent automatically every minute.");
        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok();
        //        // Log the error


        //    }
        //    // Do nothing, the job will be scheduled to run automatically
        //    //SendEmail("mhieu1140@gmail.com");
        //}
        public async Task SendEmail(string toEmail)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("kuti.hieu01@gmail.com"));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = "Test 2 Email Subject";
                email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Example 2 HTML Message Body</h1>" };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("kuti.hieu01@gmail.com", "bqwxfzwoywzwrpai");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ScheduleSendSubscriptionEmailsJob()
        {
            // Create a new scheduler
            var scheduler = await new StdSchedulerFactory().GetScheduler();

            // Schedule the recurring job to run every minute
            await scheduler.ScheduleJob(new JobDetailImpl("send-subscription-emails", typeof(SendSubscriptionEmailsJob)),
                TriggerBuilder.Create()
                    .WithIdentity("send-subscription-emails-trigger")
                    .WithCronSchedule("0 0/1 * * * ?") // every minute
                    .Build());

            // Start the scheduler
            await scheduler.Start();
        }
    }

    public class SendSubscriptionEmailsJob : IJob
    {
        private readonly IConfiguration _configuration;

        public SendSubscriptionEmailsJob(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                List<Subscription> subscriptions = new List<Subscription>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_sendMail", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@stmttype", "sendmail");

                    connection.Open();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        Subscription subscription = new Subscription();
                        subscription.Email = reader["Email"].ToString();
                        subscription.EventName = reader["EventName"].ToString();
                        subscription.Location = reader["Location"].ToString();
                        subscription.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);

                        subscriptions.Add(subscription);
                    }

                    reader.Close();
                }

                foreach (Subscription subscription in subscriptions)
                {
                    if (DateTime.Now.Date == subscription.ExpirationDate.Date.AddDays(-1))
                    {
                        // Send an email to the subscriber
                        await SendEmail(subscription.Email, subscription.EventName, subscription.Location, subscription.ExpirationDate);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw ex;
            }
        }

        public async Task SendEmail(string toEmail,string name, string location, DateTime date)
        {
            try
            {
                
                var timeAndLocation = $"{date.ToString("dddd, dd MMMM yyyy")} at {location}"; // Format the date and concatenate with the location

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("kuti.hieu01@gmail.com"));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = $"{name} is Almost Here";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $@"
            <body style=""margin: 0; padding: 0"">
                <div
                  style=""
                    border-radius: 10px;
                    box-shadow: rgba(0, 0, 0, 0.24) 0px 3px 8px;
                    margin: 0 auto;
                    margin-top: 1rem;
                    max-width: 80%;
                    padding: 1rem 2rem;
                  ""
                >
                  <h2 style=""text-align: center"">
                    <span style=""color: #0ea5e9;"">{name}</span> is Almost Here🔥🔥🔥
                  </h2>
                  <p>
                    What you waiting for? <span style=""color: #0ea5e9;"">{name}</span> you
                    subscribed is almost here pack your sefl up and join us at
                    <span style=""color: #0ea5e9;"">{timeAndLocation}</span>. We can't wait to see you there🎉🎉🎉
                  </p>
                  <p>
                    If you have any concerns, feel free to reach out to us at 
                    <span style=""color: #0ea5e9;"">👉NhaTrangFestival@gmail.com</span>
                  </p>
                </div>
              </body>"
                };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("kuti.hieu01@gmail.com", "bqwxfzwoywzwrpai");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Subscription
    {
        public string Email { get; set; }

        public string EventName { get; set; }
        public string Location { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}