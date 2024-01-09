using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.DAL.Interfaces
{
    public interface IEmployeeUploadDAL
    {
        IEnumerable<EmployeeUploadModel> GetAllByEmployeeId(short employeeId);
    }
}