using System;
using System.Collections.Generic;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Infrastructure.Comparers
{
    public class UserClaimEqualityComparer : IEqualityComparer<UserClaim>
    {
        public bool Equals(UserClaim x, UserClaim y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.UserId.Equals(y.UserId) && x.ClaimEntityId == y.ClaimEntityId;
        }

        public int GetHashCode(UserClaim obj)
        {
            return HashCode.Combine(obj.UserId, obj.ClaimEntityId);
        }
    }
}
