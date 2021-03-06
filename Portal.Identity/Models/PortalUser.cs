﻿using System;
using Dapper;
using DapperExtensions;
using Portal.Identity.DapperExtensions;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Identity.Models
{
    public class PortalUser : IUser<int>
    {
        public Task<ClaimsIdentity> GenerateUserIdentityAsync(PortalUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }

        public ClaimsIdentity GenerateUserIdentity(PortalUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity<PortalUser, int>(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // Id is key by default
        // [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string ApplicationName { get; set; }

    }
}
