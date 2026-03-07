using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TPI_GESTION_HOGAR.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendPasswordResetAsync(string toEmail, string resetLink)
        {
            var subject = "Recuperar contraeña";
            var body = $@"
                <h2>Recuperar contraseña</h2>
                <p>Hacé click en el siguiente enlace para restablecer tu contraseña:</p>
                <a href='{resetLink}'>Restablecer contraseña</a>
                <p>Si no solicitaste esto, ignorá este email.</p>
            ";
            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendNotificationAsync(string toEmail, string subject, string message)
        {
            var body = $@"
                <h2>{subject}</h2>
                <p>{message}</p>
            ";
            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
