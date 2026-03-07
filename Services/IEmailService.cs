namespace TPI_GESTION_HOGAR.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetAsync(string toEmail, string resetLink);
        Task SendNotificationAsync(string toEmail, string subject, string message);
    }
}
