using System.Net.Mail;
using System.Net;

namespace Server.Services
{
    internal class EmailSender
    {
        string fromAddress = Environment.GetEnvironmentVariable("EmailUsername");
        string fromPassword = Environment.GetEnvironmentVariable("EmailSTMP");
        public void SendMail(string toAddress, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromAddress, fromPassword),
                    EnableSsl = true,
                };

                smtpClient.Send(fromAddress, toAddress, subject, body);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send the email. Error: " + ex.Message);
            }
        }
    }
}
