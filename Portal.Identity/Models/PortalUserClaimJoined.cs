using System;
using Portal.Identity.DapperExtensions;
using Microsoft.AspNet.Identity;
using Dapper;
using DapperExtensions;

namespace Portal.Identity.Models
{
    // DO NOT user this for inserts - use PortalUserClaim
    // this is just for inner join select
    public class PortalUserClaimJoined
    {
        [Key]
        public int ClaimId { get; set; }
        [Key]
        public int UserId { get; set; }

        public int ClaimTypeId { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimValueType { get; set; }
        public string Issuer { get; set; }

        public string ClaimTypeCode { get; set; }

    }
 
}

