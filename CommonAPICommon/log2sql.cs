using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPICommon
{
    public class log2sql
    {
        IConfiguration Configuration { get; set; }

        private string Logger = string.Empty;

        private string[] CurrentModes = { };

        public log2sql(string Logger)
        {
            // Init from Config File
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();

            // Logger is passed once per New
            this.Logger = Logger;

            // Get the Modes allowed to write to Sql
            CurrentModes = Configuration["Log:Logging:CurrentLoggingModes"].Split(',');
        }

        public void Error(string message, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            if (CurrentModes.Contains("ERROR"))
                Log(caller, "ERROR", message, ex == null ? "" : $"{ex.Message} StackTrace-> {ex.StackTrace}");
        }
        public void Error(string message, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            if (CurrentModes.Contains("ERROR"))
                Log(caller, "ERROR", message, null);
        }
        public void Error(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            if (CurrentModes.Contains("ERROR"))
                Log(caller, "ERROR", ex.Message, ex == null ? "" : $"{ex.Message} StackTrace-> {ex.StackTrace}");
        }
        public void Debug(string message, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            if (CurrentModes.Contains("DEBUG"))
                Log(caller, "DEBUG", message, ex == null ? "" : $"{ex.Message} StackTrace-> {ex.StackTrace}");
        }
        public void Debug(string message, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            if (CurrentModes.Contains("DEBUG"))
                Log(caller, "DEBUG", message, null);
        }
        public void Info(string message, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            if (CurrentModes.Contains("INFO"))
                Log(caller, "INFO", message, ex == null ? "" : $"{ex.Message} StackTrace-> {ex.StackTrace}");
        }

        private void Log(string method, string type, string message, string exception)
        {
            // Pull from Config File
            string connectionString = Configuration["Log:Logging:ConnectionString"];

            // Open the Sql Connection
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            sqlConnection1.Open();

            // Insert record into Table
            SqlCommand cmd = new SqlCommand("INSERT INTO Log (Date, Level, Logger, Method, Message, Exception) VALUES (@value1, @value2, @value3, @value4, @value5, @value6)", sqlConnection1);
            cmd.Parameters.AddWithValue("@value1", DateTime.Now);
            cmd.Parameters.AddWithValue("@value2", type);
            cmd.Parameters.AddWithValue("@value3", Logger);
            cmd.Parameters.AddWithValue("@value4", method);
            cmd.Parameters.AddWithValue("@value5", message);
            cmd.Parameters.AddWithValue("@value6", exception);

            // Execute the Insert
            cmd.ExecuteNonQuery();

            // Cleanup
            cmd.Connection.Close();
            sqlConnection1.Close();
        }
    }
}