using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDepartmentDAL _departmentDAL;
        private readonly MapperBase<Department, DepartmentModel> _departmentMapper;

        public DepartmentRepository(DepartmentMapper departmentMapper, IDepartmentDAL departmentDAL)
        {
            _departmentMapper = departmentMapper;
            _departmentDAL = departmentDAL;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            IEnumerable<DepartmentModel> departmentModels = await _departmentDAL.GetAllAsync();
            return _departmentMapper.MapDataModelsToEntities(departmentModels);
        }
    }
}
