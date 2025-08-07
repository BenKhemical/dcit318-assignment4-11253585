using System.Data.SqlClient;

namespace MedicalAppointmentApp
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetConnection()
        {
            // Replace YOUR_SERVER_NAME if not using default SQLEXPRESS
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=MedicalDB;Integrated Security=True;TrustServerCertificate=True";
            return new SqlConnection(connectionString);
        }
    }
}
