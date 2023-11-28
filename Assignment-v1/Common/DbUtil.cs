using System.Data.SqlClient;
using System.Data;

namespace Assignment_v1.Common
{
    internal class DbUtil
    {
        private readonly string _connectionString;

        public DbUtil(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetData(string sqlQuery, List<SqlParameter> queryParameters)
        {
            DataTable dataTable = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
            {
                sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);

                return dataTable;
            }
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
