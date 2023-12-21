using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Infrastructure.DAL
{
    public class DataAccess
    {
        private readonly SqlConnection _connection;

        public DataAccess(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException($"{nameof(connectionString)}: No connection string provided.");
            }
            _connection = new SqlConnection(connectionString);
        }

        public int ExecuteNonQuery(string sqlQuery, List<SqlParameter> queryParameters)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    SafelyOpenConnection(_connection);
                    return sqlCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                SafelyCloseConnection(_connection);
            }
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string sqlQuery, List<SqlParameter> queryParameters)
        {
            List<Dictionary<string, object>> entityDicts = new List<Dictionary<string, object>>();

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    SafelyOpenConnection(_connection);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            entityDicts.Add(row);
                        }
                    }
                }
            }
            finally
            {
                SafelyCloseConnection(_connection);
            }

            return entityDicts;
        }

        public object ExecuteScalar(string sqlQuery, List<SqlParameter> queryParameters)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    SafelyOpenConnection(_connection);
                    return sqlCommand.ExecuteScalar();
                }
            }
            finally
            {
                SafelyCloseConnection(_connection);
            }
        }

        private void SafelyOpenConnection(DbConnection connection)
        {
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        private void SafelyCloseConnection(DbConnection connection)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
