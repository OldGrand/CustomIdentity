using System;
using System.Collections.Generic;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Infrastructure.Comparers
{
    public class RoleEqualityComparer : IEqualityComparer<Role>
    {
        public bool Equals(Role x, Role y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id && x.Title == y.Title;
        }

        public int GetHashCode(Role obj)
        {
            return HashCode.Combine(obj.Id, obj.Title);
        }
    }
}
