using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task AddOrUpdateClaimsToRoleAsync(int roleId, IEnumerable<int> claimIds);
        IAsyncEnumerable<RoleClaim> GetClaimsForRoleAsync(int roleId);
        Task CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(int roleId);
        IAsyncEnumerable<Role> GetAllRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        IAsyncEnumerable<Role> GetRolesForUser(Guid userId);
        Task AddOrUpdateUserRolesAsync(UserRolesUpdateModel model);
    }
}