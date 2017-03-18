using System.Net;
using System.Net.Mail;

namespace MyIP
{
    public class EmailSender
    {
        private const string SenderAddress = "myip@t.pl";
        private const string Pass = "h2h4w";
        private const string ReciptienAddress = "boguslaw.boczkowski@gmail.com";
        private const string EmailHost = "poczta.t.pl";
        private const int EmailPort = 587;
        public static void SendEmail(string subject, string body)
        {
            var mail = new MailMessage(SenderAddress, ReciptienAddress);
            var client = new SmtpClient
            {
                Port = EmailPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = EmailHost,
                Credentials = new NetworkCredential(SenderAddress, Pass)
            };
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
        }
    }
}
