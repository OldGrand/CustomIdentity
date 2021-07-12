using System;
using System.Collections.Generic;

namespace CustomIdentity.BLL.Models.Permissions
{
    public class UserRolesUpdateModel
    {
        public Guid UserId { get; set; }
        public IEnumerable<int> RoleIds { get; set; }
    }
}
