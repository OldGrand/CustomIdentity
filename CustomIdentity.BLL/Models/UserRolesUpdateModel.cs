using System;
using System.Collections.Generic;

namespace CustomIdentity.BLL.Models
{
    public class UserRolesUpdateModel
    {
        public Guid UserId { get; set; }
        public IEnumerable<int> RoleIds { get; set; }
    }
}
