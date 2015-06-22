using Dapper;
using DapperExtensions;
//using Portal.Identity.DapperExtensions;
using Microsoft.Owin.Security.Provider;
using Portal.Identity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Portal.Identity
{
    public class PortalUserStore :
        IUserLoginStore<PortalUser, int>,
        IUserClaimStore<PortalUser, int>,
        IUserPasswordStore<PortalUser, int>,
        IUserRoleStore<PortalUser, int>,
        IUserSecurityStampStore<PortalUser, int>,
        IUserEmailStore<PortalUser, int>,
        IUserPhoneNumberStore<PortalUser, int>,
        IUserTwoFactorStore<PortalUser, int>,
        IUserLockoutStore<PortalUser, int>,
        IDisposable
    {
        //FIXME: ? Perhaps use this key in Claims - have seen as ValueType (??)
        //private readonly string ProviderNameKey = "DapperDbProvider";

        private readonly DbConnection _sqlConn;

        public IDbConnection GetOpenConnection(string connection)
        {
            var dbConnection = new SqlConnection(connection);
            dbConnection.Open();
            return dbConnection;
        }

        //public PortalUserStore(string connectionString)
        //{
        //    _sqlConn = GetOpenConnection(connectionString);
        //}

        //#region IUserStore

        public Task CreateAsync(PortalUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                if (user != null)
                {
                    _sqlConn.Insert(user);
                }
            });
        }

        public PortalUserStore(DbConnection connection)
        {
            _sqlConn = connection;
        }
        #region IUserStore

        public static PortalUserStore Create(IdentityFactoryOptions<PortalUserStore> options, IOwinContext context)
        {
            return new PortalUserStore(context.Get<DbConnection>());
        }

        public Task UpdateAsync(PortalUser user)
        {

            return Task.Factory.StartNew(() =>
            {
                if (user != null)
                {
                    _sqlConn.Update(user);
                }
            });

        }

        public Task DeleteAsync(PortalUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                if (user != null)
                {
                    _sqlConn.Delete(user);
                }
            });
        }

        Task<PortalUser> IUserStore<PortalUser, int>.FindByIdAsync(int userId)
        {
            return userId == 0 ? null : Task.Factory.StartNew(() => _sqlConn.Get<PortalUser>(userId));
        }

        Task<PortalUser> IUserStore<PortalUser, int>.FindByNameAsync(string userName)
        {
            var predicate = Predicates.Field<PortalUser>(f => f.UserName, Operator.Eq, userName);
            return Task.Factory.StartNew(() => _sqlConn.GetList<PortalUser>(predicate).FirstOrDefault());
        }
        Task<PortalUser> FindByEmailAsync(string email)
        {
            var predicate = Predicates.Field<PortalUser>(f => f.Email, Operator.Eq, email);
            return Task.Factory.StartNew(() => _sqlConn.GetList<PortalUser>(predicate).FirstOrDefault());
        }

        #endregion

        #region IUserEmailStore
        public Task SetEmailAsync(PortalUser user, string email)
        {
            return Task.Factory.StartNew(() => user.Email = email);
        }

        public Task<string> GetEmailAsync(PortalUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(PortalUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(PortalUser user, bool confirmed)
        {
            return Task.Factory.StartNew(() => user.EmailConfirmed = confirmed);
        }
    
        Task<PortalUser> IUserEmailStore<PortalUser, int>.FindByEmailAsync(string email)
        {

            var predicateUser = Predicates.Field<PortalUser>(f => f.Email, Operator.Eq, email);
            return Task.Factory.StartNew(() => _sqlConn.GetList<PortalUser>(predicateUser).FirstOrDefault());

        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(PortalUser user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(PortalUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(PortalUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
        #endregion


        #region IUserRoleStore
        public Task AddToRoleAsync(PortalUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                var predicateRole = Predicates.Field<PortalRole>(f => f.Name, Operator.Eq, roleName);
                var roleItem = _sqlConn.GetList<PortalRole>(predicateRole).FirstOrDefault();

                if (roleItem != null)
                {
                    //does this user & role combo already exist?
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<PortalUserRole>(f => f.UserId, Operator.Eq, user.Id));
                    pg.Predicates.Add(Predicates.Field<PortalUserRole>(f => f.RoleId, Operator.Eq, roleItem.Id));
                    var roleUserItem = _sqlConn.GetList<PortalUserRole>(pg).FirstOrDefault();

                    if (roleUserItem == null)
                    {
                        // no - so add
                        _sqlConn.Insert(new PortalUserRole { UserId = user.Id, RoleId = roleItem.Id });
                    }

                }
            });

            
        }

        public Task RemoveFromRoleAsync(PortalUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                var predicateRole = Predicates.Field<PortalRole>(f => f.Name, Operator.Eq, roleName);
                var roleItem = _sqlConn.GetList<PortalRole>(predicateRole).FirstOrDefault();

                if (roleItem != null)
                {
                    //does this user & role combo already exist?
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<PortalUserRole>(f => f.UserId, Operator.Eq, user.Id));
                    pg.Predicates.Add(Predicates.Field<PortalUserRole>(f => f.RoleId, Operator.Eq, roleItem.Id));
                    var roleUserItem = _sqlConn.GetList<PortalUserRole>(pg).FirstOrDefault();

                    if (roleUserItem != null)
                    {
                        // yes - so delete
                        _sqlConn.Delete(roleUserItem);
                    }

                }
            });

        }

        public Task<IList<string>> GetRolesAsync(PortalUser user)
        {



            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                //does this user & role combo already exist?
                var results = _sqlConn.Query<PortalRole>(@"SELECT PortalRole.* FROM PortalUserRole ru
                             INNER JOIN PortalRole on PortalRole.Id = ru.RoleId WHERE ru.UserId = @Id", new { Id = user.Id }).ToList();

                var retList = results.Select(r => r.Name).ToList();
                return (IList<string>)retList;
            });
        }

        public Task<bool> IsInRoleAsync(PortalUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                var result = false;
                var predicateRole = Predicates.Field<PortalRole>(f => f.Name, Operator.Eq, roleName);
                var roleItem = _sqlConn.GetList<PortalRole>(predicateRole).FirstOrDefault();
                if (roleItem != null)
                {
                    //does this user & role combo already exist?
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<PortalUserRole>(f => f.UserId, Operator.Eq, user.Id));
                    pg.Predicates.Add(Predicates.Field<PortalUserRole>(f => f.RoleId, Operator.Eq, roleItem.Id));
                    var roleUserItem = _sqlConn.GetList<PortalUserRole>(pg).FirstOrDefault();

                    if (roleUserItem != null)
                    {
                        result = true;
                    }
                }
                return result;

            });
        }
        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(PortalUser user, string stamp)
        {
            return Task.Factory.StartNew(() => user.SecurityStamp = stamp);
        }

        public Task<string> GetSecurityStampAsync(PortalUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(PortalUser user, PortalUserLogin login)
        {
            return AddLoginAsync(user, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        }
        public Task AddLoginAsync(PortalUser user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<PortalUser>(user.Id);
                if (userItem != null)
                {
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<PortalUserLogin>(f => f.UserId, Operator.Eq, user.Id));
                    pg.Predicates.Add(Predicates.Field<PortalUserLogin>(f => f.ProviderKey, Operator.Eq, login.ProviderKey));
                    var userLoginItem = _sqlConn.GetList<PortalUserLogin>(pg).FirstOrDefault();

                    if (userLoginItem == null)
                    {
                        _sqlConn.Insert(
                            new PortalUserLogin
                            {
                                UserId = user.Id,
                                ProviderKey = login.ProviderKey,
                                LoginProvider = login.LoginProvider
                            });
                    }
                }
            });
        }
    
        public Task RemoveLoginAsync(PortalUser user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<PortalUser>(user.Id);
                if (userItem != null)
                {
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<PortalUserLogin>(f => f.UserId, Operator.Eq, user.Id));
                    pg.Predicates.Add(Predicates.Field<PortalUserLogin>(f => f.ProviderKey, Operator.Eq, login.ProviderKey));

                    var userLoginItem = _sqlConn.GetList<PortalUserLogin>(pg).FirstOrDefault();

                    if (userLoginItem != null)
                    {
                        _sqlConn.Delete(userLoginItem);
                    }
                }
            });
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(PortalUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                List<UserLoginInfo> logins = null;
                var userItem = _sqlConn.Get<PortalUser>(user.Id);
                if (userItem != null)
                {
                    var predicate = Predicates.Field<PortalUserLogin>(f => f.UserId, Operator.Eq, user.Id);
                    logins = _sqlConn.GetList<PortalUserLogin>(predicate).Select(culi => new UserLoginInfo(culi.LoginProvider, culi.ProviderKey)).ToList();

                }

                return (IList<UserLoginInfo>)logins;
            });

        }

        public Task<PortalUser> FindAsync(UserLoginInfo login)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                PortalUser user = null;
                var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pg.Predicates.Add(Predicates.Field<PortalUserLogin>(f => f.LoginProvider, Operator.Eq, login.LoginProvider));
                pg.Predicates.Add(Predicates.Field<PortalUserLogin>(f => f.ProviderKey, Operator.Eq, login.ProviderKey));

                var userLoginItem = _sqlConn.GetList<PortalUserLogin>(pg).FirstOrDefault();

                if (userLoginItem != null)
                {
                    user = _sqlConn.Get<PortalUser>(userLoginItem.UserId);
                }

                return (PortalUser)user;
            });
        }

        #endregion

        #region IUserClaimStore Helpers

        public Task<IList<PortalUserClaimType>> GetClaimTypes()
        {
            return Task.Factory.StartNew(() =>
            {
                var claimTypes = _sqlConn.Query<PortalUserClaimType>("select * from PortalUserClaimType").ToList();

                return (IList<PortalUserClaimType>)claimTypes;
            });
        }

        #endregion

        #region IUserClaimStore
        public Task<IList<Claim>> GetClaimsAsync(PortalUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                List<Claim> claims = null;
                var userItem = _sqlConn.Get<PortalUser>(user.Id);
                if (userItem != null)
                {
                    claims = _sqlConn.Query<PortalUserClaimJoined>("SELECT cuc.*, cuct.ClaimTypeCode FROM PortalUserClaim cuc "+
                                " INNER JOIN PortalUserClaimType cuct ON cuc.ClaimTypeId = cuct.TypeId WHERE cuc.UserId = @Id;",
                    new { user.Id }).Select(cuc =>
                        new Claim(cuc.ClaimTypeCode, cuc.ClaimValue, cuc.ClaimValueType, cuc.Issuer))
                           .ToList();
                }

                return (IList<Claim>)claims;
            });
        }

        public Task AddClaimAsync(PortalUser user, Claim claim)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<PortalUser>(user.Id);
                if (userItem != null)
                {

                    var oldClaim =
                        _sqlConn.Query<PortalUserClaim>(
                            "SELECT cuc.* FROM PortalUserClaim cuc "+
                               "INNER JOIN PortalUserClaimType cuct ON cuc.ClaimTypeId = cuct.TypeId "+
                               "WHERE cuc.UserId = @Id AND " +
                                   "cuct.ClaimTypeCode = @Type AND " +
                                   "cuc.ClaimValue = @Value AND " +
                                   "cuc.Issuer = @Issuer;",
                            new { user.Id, claim.Type, claim.Value, claim.Issuer })
                            .FirstOrDefault();
                    if (oldClaim == null)
                    {
                        // verify ClaimType
                        var predicate = Predicates.Field<PortalUserClaimType>(f => f.ClaimTypeCode, Operator.Eq, claim.Type);
                        var theClaimType = _sqlConn.GetList<PortalUserClaimType>(predicate).FirstOrDefault();

                        if (theClaimType != null)
                        {
                            _sqlConn.Insert<PortalUserClaim>(new PortalUserClaim
                            {
                                UserId = user.Id,
                                ClaimTypeId = theClaimType.TypeId,
                                ClaimValue = claim.ValueType,
                                ClaimValueType = claim.ValueType,
                                Issuer = claim.Issuer
                            });
                        }
                    }
                }
                return Task.FromResult(0);
            });
        }

        public Task RemoveClaimAsync(PortalUser user, Claim claim)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<PortalUser>(user.Id);
                if (userItem != null)
                {
                    var predicate = Predicates.Field<PortalUserClaimType>(f => f.ClaimTypeCode, Operator.Eq, claim.Type);
                    var theClaimType = _sqlConn.GetList<PortalUserClaimType>(predicate).FirstOrDefault();

                    if (theClaimType != null)
                    {


                        var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                        pg.Predicates.Add(Predicates.Field<PortalUserClaim>(f => f.UserId, Operator.Eq, user.Id));
                        pg.Predicates.Add(Predicates.Field<PortalUserClaim>(f => f.ClaimTypeId, Operator.Eq, theClaimType.TypeId));
                        pg.Predicates.Add(Predicates.Field<PortalUserClaim>(f => f.ClaimValue, Operator.Eq, claim.Value));
                        var oldClaim = _sqlConn.GetList<PortalUserClaim>(pg).FirstOrDefault();
                      
                        if (oldClaim != null)
                        {
                            _sqlConn.Delete(oldClaim);
                        }
                    }
                }
            });
        }
        #endregion

        public void Dispose()
        {
            if (_sqlConn != null)
            {
                if (_sqlConn.State == ConnectionState.Open || _sqlConn.State == ConnectionState.Fetching || _sqlConn.State == ConnectionState.Executing)
                    try
                    {
                        _sqlConn.Close();
                    }
                    catch
                    {
                        //absorb bad close
                    }
                _sqlConn.Dispose();
            }

        }

        public Task SetPhoneNumberAsync(PortalUser user, string phoneNumber)
        {
            return Task.Factory.StartNew(() => user.PhoneNumber = phoneNumber);
        }

        public Task<string> GetPhoneNumberAsync(PortalUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(PortalUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(PortalUser user, bool confirmed)
        {
            return Task.Factory.StartNew(() => user.PhoneNumberConfirmed = confirmed);
        }

        public Task SetTwoFactorEnabledAsync(PortalUser user, bool enabled)
        {
            return Task.Factory.StartNew(() => user.TwoFactorEnabled = enabled);
        }

        public Task<bool> GetTwoFactorEnabledAsync(PortalUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(PortalUser user)
        {
            return Task.FromResult( new DateTimeOffset(user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc.Value : DateTime.MinValue));
        }

        public Task SetLockoutEndDateAsync(PortalUser user, DateTimeOffset lockoutEnd)
        {
            return Task.Factory.StartNew(() =>
            {
                user.LockoutEndDateUtc = lockoutEnd.DateTime;
                //user.LockoutEnabled = (user.LockoutEndDateUtc > DateTime.Now);
            });
        }

        public Task<int> IncrementAccessFailedCountAsync(PortalUser user)
        {
            return Task.Factory.StartNew(() => user.AccessFailedCount++);
        }

        public Task ResetAccessFailedCountAsync(PortalUser user)
        {
            return Task.Factory.StartNew(() => user.AccessFailedCount=0);
        }

        public Task<int> GetAccessFailedCountAsync(PortalUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(PortalUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(PortalUser user, bool enabled)
        {
            return Task.Factory.StartNew(() => user.LockoutEnabled = enabled);
        }
    }
}
