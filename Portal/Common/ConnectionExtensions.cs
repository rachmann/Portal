using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public static class ConnectionExtensions
    {
        public static SqlConnection Create(this ConnectionStringSettings settings)
        {
            var dbConnection = new SqlConnection(settings.ConnectionString);
            dbConnection.Open();
            return dbConnection;
        }
    }
}
