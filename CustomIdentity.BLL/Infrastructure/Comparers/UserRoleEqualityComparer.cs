using System;
using System.Collections.Generic;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Infrastructure.Comparers
{
    public class UserRoleEqualityComparer : IEqualityComparer<UserRole>
    {
        public bool Equals(UserRole x, UserRole y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.UserId.Equals(y.UserId) && x.RoleId == y.RoleId;
        }

        public int GetHashCode(UserRole obj)
        {
            return HashCode.Combine(obj.UserId, obj.RoleId);
        }
    }
}
