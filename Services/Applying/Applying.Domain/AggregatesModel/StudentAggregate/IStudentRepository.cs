using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate
{
    public interface IStudentRepository : IRepository<Student>
    {
        Student Add(Student student);
        Student Update(Student student);
        Task<Student> FindAsync(string StudentIdentityGuid);
        Task<Student> FindByIdAsync(string id);
    }
}
