using Core.Domain;
using System.Collections.Generic;

namespace Core.Application.Repositories
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
    }
}
