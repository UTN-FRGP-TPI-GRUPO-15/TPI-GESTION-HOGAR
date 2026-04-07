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
            var subject = "Recuperar contraseña";
            var body = $@"
            <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f8f9fc; padding:20px; font-family: Arial, sans-serif;'>
                <tr>
                    <td align='center'>

                        <table width='500' cellpadding='0' cellspacing='0' style='background-color:#ffffff;'>

                            <!-- Header -->
                            <tr>
                                <td style='background-color:#6f42c1; padding:20px; text-align:center; color:white;'>
                                    <h2 style='margin:0;'>Recuperar contraseña</h2>
                                </td>
                            </tr>

                            <!-- Body -->
                            <tr>
                                <td style='padding:25px; text-align:center; color:#333;'>

                                    <p style='font-size:15px; margin:0 0 15px 0;'>
                                        Recibimos una solicitud para restablecer tu contraseña.
                                    </p>

                                    <p style='font-size:15px; margin:0 0 20px 0;'>
                                        Hacé click en el botón para continuar:
                                    </p>

                                    <a href='{resetLink}' 
                                       style='display:inline-block; padding:12px 20px; 
                                              background-color:#6f42c1; color:#ffffff; 
                                              text-decoration:none; font-weight:bold;'>
                                        Restablecer contraseña
                                    </a>

                                    <p style='margin-top:20px; font-size:12px; color:#999; word-break:break-all;'>
                                        Si el botón no funciona, copiá y pegá este enlace:<br/>
                                        {resetLink}
                                    </p>

                                    <p style='margin-top:20px; font-size:13px; color:#777;'>
                                        Si no solicitaste este cambio, podés ignorar este email.
                                    </p>

                                </td>
                            </tr>

                            <!-- Footer -->
                            <tr>
                                <td style='background-color:#f1eaff; padding:15px; text-align:center; font-size:12px; color:#666;'>
                                    Hogar de Protección Integral
                                </td>
                            </tr>

                        </table>

                    </td>
                </tr>
            </table>
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
