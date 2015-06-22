using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Portal.Identity.DapperExtensions;
using Microsoft.AspNet.Identity;
using Portal.Identity.Models;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

// use
// http://www.asp.net/identity/overview/extensibility/change-primary-key-for-users-in-aspnet-identity

namespace Portal.Identity
{
    public class PortalUserManager : UserManager<PortalUser, int>
    {
        public PortalUserManager(IUserStore<PortalUser, int> store)
            : base(store)
        {
            
        }

        //public static PortalUserManager Create(string connectionString)
        //{
        //    IUserStore<PortalUser, int> store = new PortalUserStore<PortalUser>(connectionString);
        //    return new PortalUserManager(store);

        //}
        public static PortalUserManager Create(IdentityFactoryOptions<PortalUserManager> options, IOwinContext context)
        {

            // var manager = new PortalUserManager(new PortalUserStore(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()));

            // now depend on OwinContext as setup in Statup.ConfigureAuth in App_Start\Startup.Auth.cs
            var manager = new PortalUserManager(new PortalUserStore(context.Get<DbConnection>()));

            manager.UserValidator = new UserValidator<PortalUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
               
            };

            manager.ClaimsIdentityFactory = new PortalClaimsIdentityFactory();
            manager.EmailService = new EmailService();
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<PortalUser, int>
                {
                    MessageFormat = "Your security code is: {0}"
                });
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<PortalUser, int>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                }); 
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<PortalUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public List<PortalUserClaimType> GetClaimTypes()
        {
            return (List<PortalUserClaimType>)((PortalUserStore)Store).GetClaimTypes().Result;

        }
   
    }
}
