using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Extensions;
using CustomIdentity.BLL.Infrastructure.Comparers;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly CustomIdentityDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<UserRole> _userRoles;
        private readonly DbSet<RoleClaim> _roleClaims;
        private readonly DbSet<UserClaim> _userClaims;

        public RoleService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roles = _dbContext.Roles;
            _userRoles = _dbContext.UserRoles;
            _roleClaims = _dbContext.RoleClaims;
            _userClaims = _dbContext.UserClaims;
        }

        public async Task SetOrRemoveClaimsToRoleAsync(int roleId, IEnumerable<int> claimIds)
        {
            var claimsIdList = claimIds?.ToList();

            if (claimsIdList == null || !claimsIdList.Any())
            {
                throw new ArgumentNullException(nameof(claimIds));
            }

            var roleEntity = await _roles.FirstAsync(r => r.Id == roleId);

            var isAllNewClaimIdsExist = await _userClaims.AllAsync(x => claimsIdList.Contains(x.Id));
            if (!isAllNewClaimIdsExist)
            {
                throw new Exception("Таких клаймов нету");
            }

            var existedRoleClaims = await GetClaimsForRoleAsync(roleEntity.Id).ToListAsync();
            var claimIdsToRoleClaim = claimsIdList.Select(c => new RoleClaim
            {
                RoleId = roleEntity.Id,
                UserClaimId = c
            }).ToList();

            var roleClaimEqualityComparer = new RoleClaimEqualityComparer();

            var claimsToDelete = existedRoleClaims.Except(claimIdsToRoleClaim, roleClaimEqualityComparer);
            _roleClaims.RemoveRange(claimsToDelete);

            var claimsToCreate = claimIdsToRoleClaim.Except(existedRoleClaims);
            await _roleClaims.AddRangeAsync(claimsToCreate);
        }

        public IAsyncEnumerable<RoleClaim> GetClaimsForRoleAsync(int roleId)
        {
            var claimsForRole = _roleClaims.Where(rc => rc.RoleId == roleId)
                .AsAsyncEnumerable();

            return claimsForRole;
        }

        public ValueTask<EntityEntry<Role>> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var newRoleEntity = new Role
            {
                Title = roleName
            };

            return _roles.AddAsync(newRoleEntity);
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var existedRole = await _roles.FirstOrDefaultAsync(r => r.Title == roleName);

            _roles.Remove(existedRole);
        }

        public IAsyncEnumerable<Role> GetAllRolesAsync()
        {
            var roleEntities = _roles.AsAsyncEnumerable();
            return roleEntities;
        }

        public Task<Role> GetRoleAsync(int roleId)
        {
            var taskRoleEntity = _roles.FirstOrDefaultAsync(r => r.Id == roleId);
            return taskRoleEntity;
        }
        
        public IAsyncEnumerable<Role> GetRolesForUser(Guid userId)
        {
            var userWithIncludedRoles = _userRoles.Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .AsAsyncEnumerable();

            return userWithIncludedRoles;
        }
        
        public async Task AssignRangeOfRolesToUserAsync(Guid userId, IEnumerable<int> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
            {
                throw new ArgumentNullException(nameof(roleIds));
            }

            var userEntity = await _userManager.FindByIdAsync(userId.ToString());

            var existedRolesCount = _userClaims.Count(x => roleIds.Contains(x.Id));
            if (roleIds.Count() != existedRolesCount)
            {
                throw new Exception("Таких ролей нету");
            }

            var existedUserRoleIds = await _userRoles.Where(ur => ur.UserId == userEntity.Id)
                .Select(rc => rc.RoleId)
                .ToListAsync();

            var uniqueNewRoleIds = roleIds.Except(existedUserRoleIds);

            var newUserRoleEntities = uniqueNewRoleIds.Select(r => new UserRole
            {
                RoleId = r,
                UserId = userEntity.Id
            });

            await _userRoles.AddRangeAsync(newUserRoleEntities);
        }
    }
}
