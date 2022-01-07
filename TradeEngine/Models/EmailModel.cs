using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using absSettings = TradeAbstractions.Helpers.Settings;
using helpMe = TradeEngine.Models.HelpMe;
namespace TradeEngine.Models
{
    public class EmailModel
    {
        internal void SendEmail(string subject, string body)
        {
            // Geliştirme ortamı için veya mail server'ı pasif yapmak için kullanırız.
            if (!absSettings.EngineSettings.EmailSettings.Active) return;

            string fromEmail = absSettings.EngineSettings.EmailSettings.FromEmail;
            string fromName = absSettings.EngineSettings.EmailSettings.FromName;

            string userName = absSettings.EngineSettings.EmailSettings.Username;
            string password = absSettings.EngineSettings.EmailSettings.Password;

            string host = absSettings.EngineSettings.EmailSettings.Host;
            int port = absSettings.EngineSettings.EmailSettings.Port;

            MailMessage message = new MailMessage();
            message.Subject = "Engine - " + subject;
            message.Body = body;
            message.Priority = MailPriority.High;

            message.From = new MailAddress(fromEmail, fromName, System.Text.Encoding.UTF8);

            int toCount = absSettings.EngineSettings.EmailSettings.ToEmails.Count;
            for (int i = 0; i < toCount; i++)
            {
                message.To.Add(absSettings.EngineSettings.EmailSettings.ToEmails[i]);
            }

            SmtpClient client = new SmtpClient(host, port);
            client.EnableSsl = absSettings.EngineSettings.EmailSettings.EnableSsl;

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(userName, password);

            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            try
            {
                //client.Send(message); // Bundan vazgeçtim asenkron ilerlettim
                client.SendAsync(message, null);
            }
            catch (Exception ex)
            {
                helpMe.TradeEvents.PrintMessageEvent("SendEmail, Ex: " + ex.ToString(), true);
            }
        }
        void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            // String token = (string)e.UserState;

            if (e.Cancelled)
            {
                helpMe.TradeEvents.PrintMessageEvent("Email canceled!", true);
            }
            if (e.Error != null)
            {
                helpMe.TradeEvents.PrintMessageEvent("Email Error: " + e.Error.ToString(), true);
            }
            else
            {
                helpMe.TradeEvents.PrintMessageEvent("Email sent.");
            }
        }
    }
}
