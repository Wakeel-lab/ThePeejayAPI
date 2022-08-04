using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ThePeejayAPI.DTOs;

namespace ThePeejayAPI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail(string sourceAddress, string destinationAddress, string message, string subject)
        {
            MailMessage mailMessage = new MailMessage(sourceAddress, destinationAddress, message, subject);

            using (SmtpClient client = new SmtpClient(_config["SMTP:Host"], int.Parse(_config["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_config["SMTP:Username"], _config["SMTP:Password"])
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
