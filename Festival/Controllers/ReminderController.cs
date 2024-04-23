using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;


namespace Festival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Reminder()
        {
            // Do nothing, the job will be scheduled to run automatically
            //SendEmail("mhieu1140@gmail.com");
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("kuti.hieu01@gmail.com"));
            email.To.Add(MailboxAddress.Parse("mhieu1140@gmail.com"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Example HTML Message Body</h1>" };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("kuti.hieu01@gmail.com", "bqwxfzwoywzwrpai");
            smtp.Send(email);
            smtp.Disconnect(true);
            return Ok("Subscription emails will be sent automatically every minute.");
        }
        //public async Task SendEmail(string email)
        //{
        //    try
        //    {
        //        string senderEmail = "kuti.hieu01@gmail.com";
        //        string senderPassword = "korean31";

        //        MailMessage mail = new MailMessage(senderEmail, email);
        //        SmtpClient client = new SmtpClient();
        //        client.Port = 587;
        //        client.Host = "smtp.gmail.com";
        //        client.EnableSsl = true;
        //        client.Timeout = 10000;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential(senderEmail, senderPassword);

        //        mail.Subject = "Your subscription has expired";
        //        mail.Body = "Your subscription has expired. Please renew your subscription to continue using our services.";

        //         client.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
