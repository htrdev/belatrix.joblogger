using System.Data.SqlClient;

namespace Belatrix.JobLogger
{
    public class DatabaseJobLogger
        : IJobLogger
    {
        private readonly SqlConnection _connection;

        public DatabaseJobLogger(SqlConnection connection)
        {
            _connection = connection;
        }

        public DatabaseJobLogger()
        {
            _connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        }

        public void LogMessage(string message, LogLevel logLevel)
        {
            _connection.Open();
            System.Data.SqlClient.SqlCommand command = new
                System.Data.SqlClient.SqlCommand("Insert into Log Values('" + message + "', " +
                                                 logLevel.ToString() + ")");
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}