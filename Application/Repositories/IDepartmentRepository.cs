using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
    }
}
