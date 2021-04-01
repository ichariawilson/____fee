using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplyingContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public ApplicationRepository(ApplyingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Application Add(Application application)
        {
            return _context.Applications.Add(application).Entity;

        }

        public async Task<Application> GetAsync(int applicationId)
        {
            var application = await _context
                                .Applications
                                .Include(x => x.Profile)
                                .FirstOrDefaultAsync(a => a.Id == applicationId);
            if (application == null)
            {
                application = _context
                            .Applications
                            .Local
                            .FirstOrDefault(a => a.Id == applicationId);
            }
            if (application != null)
            {
                await _context.Entry(application)
                    .Collection(i => i.ApplicationItems).LoadAsync();
                await _context.Entry(application)
                    .Reference(i => i.ApplicationStatus).LoadAsync();
            }

            return application;
        }

        public void Update(Application application)
        {
            _context.Entry(application).State = EntityState.Modified;
        }
    }
}
