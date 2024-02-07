using System.Data;
using System.Data.SqlClient;

namespace BroadcastAPI.Helper
{
    public class ConnectionHandle
    {
        public static void closeConnection(SqlConnection dbCon)
        {
            if (dbCon.State == ConnectionState.Open)
            {
                dbCon.Close();
            }
        }
        public static void openConnection(SqlConnection dbCon)
        {
            if (dbCon.State != ConnectionState.Open)
            {
                dbCon.Open();
            }
        }
    }
}
