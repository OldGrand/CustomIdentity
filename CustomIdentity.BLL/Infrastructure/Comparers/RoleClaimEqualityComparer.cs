using System;
using System.Collections.Generic;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Infrastructure.Comparers
{
    public class RoleClaimEqualityComparer : IEqualityComparer<RoleClaim>
    {
        public bool Equals(RoleClaim x, RoleClaim y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.ClaimEntityId == y.ClaimEntityId && x.RoleId == y.RoleId;
        }

        public int GetHashCode(RoleClaim obj)
        {
            return HashCode.Combine(obj.ClaimEntityId, obj.RoleId);
        }
    }
}
