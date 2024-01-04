using Core.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.DAL
{
    public class DataAccess
    {
        private readonly SqlConnection _connection;

        public DataAccess(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public int ExecuteNonQuery(string sqlQuery, List<SqlParameter> queryParameters)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    SafelyOpenConnection();
                    return sqlCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                SafelyCloseConnection();
            }
        }

        public IEnumerable<(string, object)[]> ExecuteReader(string sqlQuery, List<SqlParameter> queryParameters)
        {
            List<(string, object)[]> entityValueTuplesArrays = new List<(string, object)[]>();

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    SafelyOpenConnection();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            (string, object)[] row = new (string, object)[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = (reader.GetName(i), reader.GetValue(i));
                            }
                            entityValueTuplesArrays.Add(row);
                        }
                    }
                }
            }
            finally
            {
                SafelyCloseConnection();
            }

            return entityValueTuplesArrays;
        }

        public object ExecuteScalar(string sqlQuery, List<SqlParameter> queryParameters)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    SafelyOpenConnection();
                    return sqlCommand.ExecuteScalar();
                }
            }
            finally
            {
                SafelyCloseConnection();
            }
        }

        private void SafelyOpenConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void SafelyCloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
