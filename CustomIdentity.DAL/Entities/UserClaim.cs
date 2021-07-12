using System;

namespace CustomIdentity.DAL.Entities
{
    public class UserClaim
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int ClaimEntityId { get; set; }
        public ClaimEntity ClaimEntity { get; set; }
    }
}
