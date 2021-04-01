using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Application Add(Application application);

        void Update(Application application);

        Task<Application> GetAsync(int applicationId);
    }
}
