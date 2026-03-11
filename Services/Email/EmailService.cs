using MailKit.Security;
using MimeKit;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpAsync(string toEmail, string otpCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _config["Email:FromName"],
                _config["Email:UserName"]
            ));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Your confirmation code";

            message.Body = new TextPart("html")
            {
                Text = $"""
                    <h2>ITC App</h2>
                    <p>Your confirmation code:</p>
                    <h1 style="letter-spacing: 8px;">{otpCode}</h1>
                    <p>Code expires in <strong>10 minutes</strong>.</p>
                """
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(
                _config["Email:Host"],
                int.Parse(_config["Email:Port"]!),
                SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(
                _config["Email:UserName"],
                _config["Email:Password"]
            );
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}