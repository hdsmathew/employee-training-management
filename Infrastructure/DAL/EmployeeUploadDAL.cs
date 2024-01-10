﻿using Core.Application;
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
    public class EmployeeUploadDAL : IEmployeeUploadDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<EmployeeUpload, EmployeeUploadModel> _employeeUploadMapper;

        public EmployeeUploadDAL(DataAccess dataAccess, EmployeeUploadMapper employeeUploadMapper)
        {
            _dataAccess = dataAccess;
            _employeeUploadMapper = employeeUploadMapper;
        }

        public async Task<IEnumerable<EmployeeUploadModel>> GetAllByEmployeeIdAsync(short employeeId)
        {
            string selectQuery = @"SELECT PrerequisiteId, UploadedAt, UploadedFileName FROM EmployeeUpload WHERE EmployeeId = @EmployeeId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeId", employeeId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = await _dataAccess.ExecuteReaderAsync(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (!entityValueTuplesArrays.Any())
            {
                return new List<EmployeeUploadModel>();
            }
            return _employeeUploadMapper.MapTableToDataModels(entityValueTuplesArrays);
        }
    }
}
