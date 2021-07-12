using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task AddOrUpdateClaimsToRoleAsync(RoleClaimsUpdateModel model);
        IAsyncEnumerable<RoleClaim> GetClaimsForRoleAsync(int roleId);
        Task CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(int roleId);
        Task DeleteRolesRangeAsync(IEnumerable<int> roleIds);
        IAsyncEnumerable<Role> GetAllRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        IAsyncEnumerable<Role> GetRolesForUser(Guid userId);
        Task AddOrUpdateUserRolesAsync(UserRolesUpdateModel model);
    }
}