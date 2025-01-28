using System.Data;
using System.Data.SqlClient;

namespace FirstCrud.Data
{
    public static class DbConnectionHelper
    {
        public static IDbConnection GetConnection()
        {
            string connectionString = "Server=192.168.0.5,1777; Database=Ace4_des; User Id=Ace4Glima; Password=@G123;";
            return new SqlConnection(connectionString);
        }
    }
}
