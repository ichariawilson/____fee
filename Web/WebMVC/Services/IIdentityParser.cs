using System.Security.Principal;

namespace Microsoft.Fee.WebMVC.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
