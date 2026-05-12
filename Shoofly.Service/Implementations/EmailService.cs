using Microsoft.Extensions.Configuration;
using Shoofly.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Service.Implementations
{
    public class EmailService : IEmailService
    {
        #region Fields
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        #region Methods
        public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken)
        {
            var emailSettings = _config.GetSection("EmailSettings");
            var senderEmail = emailSettings["Email"];
            var senderPassword = emailSettings["Password"];
            var host = emailSettings["Host"];
            var port = int.Parse(emailSettings["Port"]!);

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true 
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, "Shoofly App"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Allows you to send styled HTML emails!
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage, cancellationToken);
        }
        #endregion
    }
}
