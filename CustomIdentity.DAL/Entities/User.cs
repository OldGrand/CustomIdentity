using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentity.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        
        [ProtectedPersonalData]
        public string UserName { get; set; }
        
        public string NormalizedUserName { get; set; }
        
        [ProtectedPersonalData]
        public string Email { get; set; }
        
        public string NormalizedEmail { get; set; }
        
        [PersonalData]
        public bool EmailConfirmed { get; set; }
        
        public string PasswordHash { get; set; }
        
        public string SecurityStamp { get; set; }
        
        public string ConcurrencyStamp { get; set; }

        [PersonalData]
        public bool TwoFactorEnabled { get; set; }
        
        public int AccessFailedCount { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}
