using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Student.Identity.API.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
