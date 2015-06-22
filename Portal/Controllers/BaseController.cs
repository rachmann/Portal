using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Portal.Common;
using Portal.Identity;
using Portal.Identity.Models;
using Portal.Models;

namespace Portal.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationSignInManager _signInManager;
        protected PortalUserManager _userManager;

        public BaseController()
        {
        }

        public BaseController(PortalUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public PortalUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PortalUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public string GetDbConnectionString()
        {
            return ConnectionStrings.ActiveDbConnection.ConnectionString;
        }

        public PortalLog GetLogBasics()
        {
            var ctx = HttpContext;
            var rq = ctx.Request;
            var br = rq.Browser;
            var usr = GetUser() ?? new PortalUser{ UserName = "Anonymous"};

            return new PortalLog
            {
                Browser = br.Browser,
                IP = GetIPAddress(),
                Created = DateTime.Now,
                User = usr.UserName,
                HttpMethod = rq.HttpMethod,
                ContentType = rq.ContentType,
                ContentEncoding = rq.ContentEncoding.ToString()
            };

        }
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public PortalUser GetUser()
        {
            return UserManager.FindById(User.Identity.GetUserId<int>());
        }

    }
}
