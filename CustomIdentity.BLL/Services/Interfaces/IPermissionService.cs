using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IPermissionService
    {
        IAsyncEnumerable<Role> GetRolesForUser(Guid userId);
        Task AddOrUpdateClaimsToRoleAsync(RoleClaimsUpdateModel model);
        IAsyncEnumerable<ClaimEntity> GetClaimsForRoleAsync(int roleId);
        Task AddOrUpdateUserRolesAsync(UserRolesUpdateModel model);
        Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model);
        IAsyncEnumerable<ClaimEntity> GetClaimsForUserAsync(Guid userId);
    }
}