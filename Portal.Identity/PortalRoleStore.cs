using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
//using Portal.Identity.DapperExtensions;
using Portal.Identity.Models;
using Microsoft.AspNet.Identity;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Owin.Security.Provider;


namespace Portal.Identity
{
    public class PortalRoleStore : IQueryableRoleStore<PortalRole, int>, IRoleStore<PortalRole, int>, IDisposable
    {
        private bool _disposed;
        private readonly DbConnection _sqlConn;

        public IDbConnection GetOpenConnection(string connection)
        {
            var dbConnection = new SqlConnection(connection);
            dbConnection.Open();
            return dbConnection;
        }

        public PortalRoleStore(DbConnection connection)
        {
            _sqlConn = connection;
        }

        #region IQueryableRoleStore Members
        public IQueryable<PortalRole> Roles
        {
            get
            {
                var result = this._sqlConn.GetList<PortalRole>(new { }); 
                if (result != null)
                { return result.AsQueryable();}
                else
                {
                    return new List<PortalRole>().AsQueryable();
                }
                
            }
        }

        #endregion

        #region IRoleStore Members
        public Task CreateAsync(PortalRole role)
        {
            this.ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() => _sqlConn.Insert(role));
        }

        public Task DeleteAsync(PortalRole role)
        {
            this.ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() => _sqlConn.Delete(role));
        }
        public Task UpdateAsync(PortalRole role)
        {
            this.ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.Factory.StartNew(() => _sqlConn.Update(role));
        }

        public Task<PortalRole> FindByIdAsync(int roleId)
        {
            this.ThrowIfDisposed();

            return Task.Factory.StartNew(() => _sqlConn.Get<PortalRole>(roleId));
        }

        public Task<PortalRole> FindByNameAsync(string roleName)
        {
            this.ThrowIfDisposed();
            var predicate = Predicates.Field<PortalRole>(f => f.Name, Operator.Eq, roleName);
            return Task.Factory.StartNew(() => _sqlConn.GetList<PortalRole>(predicate).FirstOrDefault());
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(
            bool disposing)
        {
            this._disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }
        #endregion

        
    }

}
