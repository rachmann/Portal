using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Portal.Identity.DapperExtensions;

namespace Portal.Identity.Models
{
    public class PortalUserRole
    {
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int UserId { get; set; }

        public virtual PortalRole PortalRole { get; set; }
        public virtual PortalUser PortalUser { get; set; }
    }
}
