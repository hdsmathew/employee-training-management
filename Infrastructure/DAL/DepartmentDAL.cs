using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
    public class DepartmentDAL : IDepartmentDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Department, DepartmentModel> _departmentMapper;

        public DepartmentDAL(DataAccess dataAccess, DepartmentMapper departmentMapper)
        {
            _dataAccess = dataAccess;
            _departmentMapper = departmentMapper;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllAsync()
        {
            string selectQuery = "SELECT DepartmentId, DepartmentName FROM Department";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }
            return _departmentMapper.MapTableToDataModels(entityValueTuplesArrays);
        }
    }
}
