using Core.Application.Repositories;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;

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

        public IEnumerable<Department> GetAll()
        {
            IEnumerable<DepartmentModel> departmentModels = _departmentDAL.GetAll();
            return _departmentMapper.MapDataModelsToEntities(departmentModels);
        }
    }
}
