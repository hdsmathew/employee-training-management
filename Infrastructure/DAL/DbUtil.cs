using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Infrastructure.DAL
{
    public class DbUtil
    {
        private readonly string _connectionString;

        public DbUtil(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int ExecuteNonQuery(string sqlQuery, List<SqlParameter> queryParameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                SafelyOpenConnection(connection);
                return sqlCommand.ExecuteNonQuery();
            }
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string sqlQuery, List<SqlParameter> queryParameters)
        {
            List<Dictionary<string, object>> entityDicts = new List<Dictionary<string, object>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                SafelyOpenConnection(connection);
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
            return entityDicts;
        }

        public object ExecuteScalar(string sqlQuery, List<SqlParameter> queryParameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                SafelyOpenConnection(connection);
                return sqlCommand.ExecuteScalar();
            }
        }

        private void SafelyOpenConnection(DbConnection connection)
        {
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}
