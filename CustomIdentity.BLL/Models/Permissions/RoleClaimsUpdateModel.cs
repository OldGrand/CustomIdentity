using System.Collections.Generic;

namespace CustomIdentity.BLL.Models.Permissions
{
    public class RoleClaimsUpdateModel
    {
        public int RoleId { get; set; }
        public IEnumerable<int> ClaimIds { get; set; }
    }
}
