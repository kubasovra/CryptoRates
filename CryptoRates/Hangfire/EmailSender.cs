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
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _configuration;
        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                _logger.LogInformation("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                _logger.LogInformation("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                _logger.LogInformation("Message sent.");
            }
        }
        public void SendEmail(string destinationEmail, string messageSubject, string messageBody)
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

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = destinationEmail + messageBody;
            client.SendAsync(message, userState);
        }
    }
}
