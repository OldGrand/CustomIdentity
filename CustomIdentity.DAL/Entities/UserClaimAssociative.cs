using System;

namespace CustomIdentity.DAL.Entities
{
    public class UserClaimAssociative
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int UserClaimId { get; set; }
        public UserClaim UserClaim { get; set; }
    }
}
