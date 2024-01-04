using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IDepartmentDAL
    {
        IEnumerable<DepartmentModel> GetAll();

    }
}
