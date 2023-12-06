using System.Collections.Generic;
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

        public bool ExecuteNonQuery(string sqlQuery, List<SqlParameter> queryParameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                connection.Open();
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string sqlQuery, List<SqlParameter> queryParameters)
        {
            List<Dictionary<string, object>> entityDicts = new List<Dictionary<string, object>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                connection.Open();
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
                connection.Open();
                return sqlCommand.ExecuteScalar();
            }
        }
    }
}
