using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Portal.Identity.Models;

namespace Portal.Identity
{
    class PortalDataProtectorTokenProvider<TPortalUser> : DataProtectorTokenProvider<PortalUser, int>
    {
        public PortalDataProtectorTokenProvider(IDataProtector protector)
            : base(protector)
        {

        }
    }
}
