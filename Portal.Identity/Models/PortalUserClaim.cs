using System;
using Dapper;
using DapperExtensions;
using Portal.Identity.DapperExtensions;
using Microsoft.AspNet.Identity;

namespace Portal.Identity.Models
{
    public class PortalUserClaim
    {
        [Key]
        public int ClaimId { get; set; }
        [Key]
        public int UserId { get; set; }

        public int ClaimTypeId { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimValueType { get; set; }
        public string Issuer { get; set; }
    }
 
}
