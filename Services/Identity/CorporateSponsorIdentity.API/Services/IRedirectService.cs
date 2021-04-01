namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Services
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
