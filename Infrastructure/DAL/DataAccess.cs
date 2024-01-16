using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
    public class DataAccess
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;

        public DataAccess()
        {
            _connectionString = ConfigurationManager.AppSettings["DefaultConnectionString"];
            _connection = new SqlConnection(_connectionString);
        }

        public async Task<int> ExecuteNonQuery(string sqlQuery, List<SqlParameter> queryParameters)
        {
            if (sqlQuery is null || queryParameters is null) throw new ArgumentNullException("Arguments cannot be null.");

            try
            {
                await SafelyOpenConnectionAsync();
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    return await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                SafelyCloseConnection();
            }
        }

        public async Task<IEnumerable<(string, object)[]>> ExecuteReaderAsync(string sqlQuery, List<SqlParameter> queryParameters)
        {
            if (sqlQuery is null || queryParameters is null) throw new ArgumentNullException("Arguments cannot be null.");

            List<(string, object)[]> entityValueTuplesArrays = new List<(string, object)[]>();

            try
            {
                await SafelyOpenConnectionAsync();
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
                return entityValueTuplesArrays;
            }
            finally
            {
                SafelyCloseConnection();
            }
        }

        public async Task<object> ExecuteScalar(string sqlQuery, List<SqlParameter> queryParameters)
        {
            if (sqlQuery is null || queryParameters is null) throw new ArgumentNullException("Arguments cannot be null.");

            try
            {
                await SafelyOpenConnectionAsync();
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection))
                {
                    sqlCommand.Parameters.AddRange(queryParameters.ToArray());
                    return await sqlCommand.ExecuteScalarAsync();
                }
            }
            finally
            {
                SafelyCloseConnection();
            }
        }

        private async Task SafelyOpenConnectionAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync();
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
