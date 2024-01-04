using Core.Application;
using Core.Domain;
using Infrastructure.Common;
using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL
{
    public class PrerequisiteDAL : IPrerequisiteDAL
    {
        private readonly DataAccess _dataAccess;
        private readonly MapperBase<Prerequisite, PrerequisiteModel> _prerequisiteMapper;

        public PrerequisiteDAL(DataAccess dataAccess, PrerequisiteMapper prerequisiteMapper)
        {
            _dataAccess = dataAccess;
            _prerequisiteMapper = prerequisiteMapper;
        }

        public int Add(PrerequisiteModel prerequisite)
        {
            string insertQuery = @"INSERT INTO Prerequisite (DocumentName)
                                   VALUES (@DocumentName);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@DocumentName", prerequisite.DocumentName)
            };
            int rowsAffected;

            try
            {
                rowsAffected = _dataAccess.ExecuteNonQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (rowsAffected == 0)
            {
                throw new DALException("No rows added");
            }
            return rowsAffected;
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
            string selectQuery = "SELECT * FROM Prerequisite";
            List<SqlParameter> parameters = new List<SqlParameter>();
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (!entityValueTuplesArrays.Any())
            {
                throw new DALException("No rows returned");
            }
            return _prerequisiteMapper.MapTableToEntities(entityValueTuplesArrays);
        }

        public IEnumerable<PrerequisiteModel> GetAllByTrainingId(short trainingId)
        {
            string selectQuery = @"WITH PrerequisiteIds (PrerequisiteId) AS (
                                        SELECT PrerequisiteId FROM TrainingPrerequisite WHERE TrainingId = @TrainingId
                                   )
                                   SELECT pr.PrerequisiteId, DocumentName
                                   FROM Prerequisite pr INNER JOIN PrerequisiteIds prIds
                                   ON pr.PrerequisiteId = prIds.PrerequisiteId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            IEnumerable<(string, object)[]> entityValueTuplesArrays;

            try
            {
                entityValueTuplesArrays = _dataAccess.ExecuteReader(selectQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new DALException("Error while executing query", ex);
            }

            if (!entityValueTuplesArrays.Any())
            {
                return new List<PrerequisiteModel>();
            }
            return _prerequisiteMapper.MapTableToEntities(entityValueTuplesArrays);
        }
    }
}
