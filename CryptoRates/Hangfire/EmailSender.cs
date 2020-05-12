using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace CryptoRates.Hangfire
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string destinationEmail, string messageSubject, string messageBody)
        {
            IConfigurationSection smtpSection = _configuration.GetSection("SmtpClient");
            SmtpClient client = new SmtpClient(smtpSection["Host"]);
            client.Port = int.Parse(smtpSection["Port"]);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(smtpSection["Username"], smtpSection["Password"]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailAddress from = new MailAddress("kubasovra@gmail.com.com", "Kubasov R.A.", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(destinationEmail);
            MailMessage message = new MailMessage(from, to);

            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = messageSubject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            await client.SendMailAsync(message);

            return true;
        }
    }
}
