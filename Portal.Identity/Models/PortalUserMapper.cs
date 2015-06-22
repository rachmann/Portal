using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using Portal.Identity.DapperExtensions;

namespace Portal.Identity.Models
{
    public class PortalUserMapper : ClassMapper<PortalUser>
    {

        public PortalUserMapper()
        {
            //Map(f => f.Id).Ignore();
            AutoMap();
        }
    }
}
