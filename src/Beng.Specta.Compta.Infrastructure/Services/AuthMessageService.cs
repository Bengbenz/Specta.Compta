using System.Threading.Tasks;

namespace Beng.Specta.Compta.Infrastructure.Services
{
    public class AuthMessageService : IEmailSender
    {
        public async Task SendEmailAsync(
            string email,
            string subject,
            string message)
        {
            await Task.CompletedTask;
        }
        
        public async Task SendEmailConfirmationAsync(
            string email,
            string userName,
            string confirmationLink)
        {
            await Task.CompletedTask;
        }

        public async Task SendPasswordForgetAsync(
            string email,
            string userName,
            string passwordResetLink)
        {
            await Task.CompletedTask;
        }
    }
}