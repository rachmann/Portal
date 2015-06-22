using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Portal.Identity;
using Portal.Identity.Models;

namespace Portal
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    //public class ApplicationUserManager : Portal.Identity.PortalUserManager
    //{
    //    public ApplicationUserManager(PortalUserStore store)
    //        : base(store)
    //    {
    //    }
    //}

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<PortalUser, int>
    {
        public ApplicationSignInManager(PortalUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(PortalUser user)
        {
            return user.GenerateUserIdentityAsync((PortalUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<PortalUserManager>(), context.Authentication);
        }
    }
}
