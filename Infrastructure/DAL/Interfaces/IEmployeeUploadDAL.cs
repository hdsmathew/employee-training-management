using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEmployeeUploadDAL
    {
        Task<IEnumerable<EmployeeUploadModel>> GetAllByEmployeeIdAsync(short employeeId);
    }
}