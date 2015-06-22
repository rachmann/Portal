using Portal.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Common;

namespace Portal.Identity
{

    public class PortalRoleManager : RoleManager<PortalRole,int>
    {
        public PortalRoleManager(IRoleStore<PortalRole, int> roleStore) : base(roleStore)
        {
        }

        public static PortalRoleManager Create(IdentityFactoryOptions<PortalRoleManager> options, IOwinContext context)
        {
            var manager = new PortalRoleManager(new PortalRoleStore(context.Get<DbConnection>()));

            return manager;
        }
    }
}
