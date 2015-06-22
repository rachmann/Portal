using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace Portal.Identity.Models
{
    public class PortalRoleMapper : ClassMapper<PortalRole>
    {

        public PortalRoleMapper()
        {
            Map(f => f.Id).Ignore();
            AutoMap();
        }
    }
}
