using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplyingContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public StudentRepository(ApplyingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Student Add(Student student)
        {
            if (student.IsTransient())
            {
                return _context.Students
                    .Add(student)
                    .Entity;
            }
            else
            {
                return student;
            }
        }

        public Student Update(Student student)
        {
            return _context.Students
                    .Update(student)
                    .Entity;
        }

        public async Task<Student> FindAsync(string identity)
        {
            var student = await _context.Students
                .Include(b => b.PaymentMethods)
                .Where(b => b.IdentityGuid == identity)
                .SingleOrDefaultAsync();

            return student;
        }

        public async Task<Student> FindByIdAsync(string id)
        {
            var student = await _context.Students
                .Include(b => b.PaymentMethods)
                .Where(b => b.Id == int.Parse(id))
                .SingleOrDefaultAsync();

            return student;
        }
    }
}
