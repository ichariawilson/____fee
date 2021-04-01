namespace Microsoft.Fee.Services.Applying.API.Application.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationQueries
    {
        Task<Application> GetApplicationAsync(int id);

        Task<IEnumerable<ApplicationSummary>> GetApplicationsFromUserAsync(Guid userId);

        Task<IEnumerable<PaymentType>> GetPaymentTypesAsync();
    }
}
