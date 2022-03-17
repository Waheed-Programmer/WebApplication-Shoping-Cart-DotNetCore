using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.CommonHelper
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var toemail = new MimeMessage();
            toemail.From.Add(MailboxAddress.Parse("Youre Email"));
            toemail.To.Add(MailboxAddress.Parse(email));
            toemail.Subject = subject;
            toemail.Body= new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };
            using(var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("UserId","Password");
                emailClient.SendAsync(toemail);
                emailClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
