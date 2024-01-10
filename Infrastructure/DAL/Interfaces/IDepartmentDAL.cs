using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IDepartmentDAL
    {
        Task<IEnumerable<DepartmentModel>> GetAllAsync();
    }
}
