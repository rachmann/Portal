﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Portal.Identity
{
    public class PortalPasswordHasher : IPasswordHasher 
    {
        public string HashPassword(string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == providedPassword)
                return PasswordVerificationResult.Success;
            return PasswordVerificationResult.Failed;

        }
    }
}
