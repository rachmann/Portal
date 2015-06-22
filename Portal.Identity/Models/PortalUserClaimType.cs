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
    public class PortalUserClaimType
    {
        [Key]
        public int TypeId { get; set; }
        public string ClaimTypeCode { get; set; }
        public string ClaimTypeDescription { get; set; }
    }
}
