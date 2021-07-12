using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IPermissionService
    {
        Task AddOrUpdateClaimsToRoleAsync(RoleClaimsUpdateModel model);
        IAsyncEnumerable<RoleClaim> GetClaimsForRoleAsync(int roleId);
        Task AddOrUpdateUserRolesAsync(UserRolesUpdateModel model);
        Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model);
        IAsyncEnumerable<UserClaim> GetClaimsForUserAsync(Guid userId);
    }
}