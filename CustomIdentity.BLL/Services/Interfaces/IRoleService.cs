using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task SetOrRemoveClaimsToRoleAsync(int roleId, IEnumerable<int> claimIds);
        IAsyncEnumerable<RoleClaim> GetClaimsForRoleAsync(int roleId);
        ValueTask<EntityEntry<Role>> CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(string roleName);
        IAsyncEnumerable<Role> GetAllRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        IAsyncEnumerable<Role> GetRolesForUser(Guid userId);
        Task AssignRangeOfRolesToUserAsync(Guid userId, IEnumerable<int> roleIds);
    }
}