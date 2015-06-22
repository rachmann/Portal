using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Portal.Identity.Models;
using Portal.Models;

namespace Portal.Common
{
    public class Db
    {
        SqlConnection GetOpenConnection(string connection)
        {
            var conn = new SqlConnection(connection);
            conn.Open();
            return conn;
        }

        public void WriteLog(string connection, PortalLog item)
        {
            using (var conn = GetOpenConnection(connection))
            {
                conn.Insert(item);
            }
        }

        public PortalUser FindUserByEmail(string connection, string email)
        {
            PortalUser user = null;
            using (var conn = GetOpenConnection(connection))
            {
                var users = conn.Query<PortalUser>("select * from PortalUser where email=@email", new {email});
                if (users != null && users.Any())
                {
                    user = users.First();
                }
            }
            return user;
        }
    }
}
