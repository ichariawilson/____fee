using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Student.Identity.API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
