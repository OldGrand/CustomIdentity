using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Extensions;
using CustomIdentity.BLL.Infrastructure.Comparers;
using CustomIdentity.BLL.Models;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddOrUpdateClaimsToRoleAsync(RoleClaimsUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var claimsIdList = model.ClaimIds?.ToList();
            if (claimsIdList == null || !claimsIdList.Any())
            {
                throw new ArgumentNullException(nameof(model.ClaimIds));
            }

            var roleEntity = await _roles.FirstAsync(r => r.Id == model.RoleId);

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

            var claimsToCreate = claimIdsToRoleClaim.Except(existedRoleClaims, roleClaimEqualityComparer);
            await _roleClaims.AddRangeAsync(claimsToCreate);
        }

        public IAsyncEnumerable<RoleClaim> GetClaimsForRoleAsync(int roleId)
        {
            var claimsForRole = _roleClaims.Where(rc => rc.RoleId == roleId)
                .AsAsyncEnumerable();

            return claimsForRole;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var newRoleEntity = new Role
            {
                Title = roleName
            };

            await _roles.AddAsync(newRoleEntity);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteRoleAsync(int roleId)
        {
            var roleToArray = new[] { roleId };
            return DeleteRolesRangeAsync(roleToArray);
        }

        public async Task DeleteRolesRangeAsync(IEnumerable<int> roleIds)
        {
            var existedRoles = await _roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();

            if (existedRoles.Count != roleIds.Count())
            {
                throw new Exception("чего-то не хватает");
            }

            _roles.RemoveRange(existedRoles);
            await _dbContext.SaveChangesAsync();
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
        
        public async Task AddOrUpdateUserRolesAsync(UserRolesUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var rolesIdList = model.RoleIds?.ToList();
            if (rolesIdList == null || !rolesIdList.Any())
            {
                throw new ArgumentNullException(nameof(model.RoleIds));
            }

            var userEntity = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (userEntity == null)
            {
                throw new ArgumentNullException("EntityNotFoundException");
            }

            var existedRolesCount = _roles.Count(x => rolesIdList.Contains(x.Id));
            if (rolesIdList.Count != existedRolesCount)
            {
                throw new Exception("Таких ролей нету");
            }

            var existedUserRoles = await _userRoles.Where(ur => ur.UserId == userEntity.Id)
                .ToListAsync();
            var rolesIdsToUserRoles = rolesIdList.Select(r => new UserRole
            {
                UserId = userEntity.Id,
                RoleId = r
            }).ToList();

            var userRoleEqualityComparer = new UserRoleEqualityComparer();

            var userRolesToDelete = existedUserRoles.Except(rolesIdsToUserRoles, userRoleEqualityComparer);
            _userRoles.RemoveRange(userRolesToDelete);

            var userRolesToCreate = rolesIdsToUserRoles.Except(existedUserRoles, userRoleEqualityComparer);
            await _userRoles.AddRangeAsync(userRolesToCreate);

            await _dbContext.SaveChangesAsync();
        }
    }
}
