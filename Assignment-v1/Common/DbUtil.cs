using System.Collections.Generic;
using System.Data.SqlClient;

namespace Assignment_v1.Common
{
    public class DbUtil
    {
        private readonly string _connectionString;

        public DbUtil(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Dictionary<string, object>> GetData(string sqlQuery, List<SqlParameter> queryParameters)
        {
            List<Dictionary<string, object>> entityTable = new List<Dictionary<string, object>>();

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

                        entityTable.Add(row);
                    }
                }
            }

            return entityTable;
        }

        public bool ModifyData(string sqlQuery, List<SqlParameter> queryParameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());

                connection.Open();
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }
    }
}
