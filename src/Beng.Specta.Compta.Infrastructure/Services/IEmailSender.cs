using System.Threading.Tasks;

namespace Beng.Specta.Compta.Infrastructure.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink);
        Task SendPasswordForgetAsync(string email, string userName, string passwordResetLink);
    }
}