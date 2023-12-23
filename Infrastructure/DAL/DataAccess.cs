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

        public IEnumerable<(string, object)[]> ExecuteReader(string sqlQuery, List<SqlParameter> queryParameters)
        {
            List<(string, object)[]> entityValueTuplesArrays = new List<(string, object)[]>();

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
                SafelyCloseConnection(_connection);
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
