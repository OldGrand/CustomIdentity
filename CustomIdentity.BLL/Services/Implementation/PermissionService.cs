using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly CustomIdentityDbContext _dbContext;
        private readonly DbSet<UserClaim> _userClaims;
        private readonly DbSet<ClaimEntity> _claimEntities;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<UserRole> _userRoles;
        private readonly UserManager<User> _userManager;

        private readonly DbSet<RoleClaim> _roleClaims;

        public PermissionService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userClaims = dbContext.UserClaims;
            _roles = dbContext.Roles;
            _userRoles = dbContext.UserRoles;
            _roleClaims = _dbContext.RoleClaims;
            _claimEntities = dbContext.ClaimEntities;
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
            if (rolesIdList == null)
            {
                throw new ArgumentNullException(nameof(model.RoleIds));
            }

            var userEntity = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (userEntity == null)
            {
                throw new ArgumentNullException("EntityNotFoundException");
            }

            var existedUserRoles = _userRoles.Where(ur => ur.UserId == userEntity.Id);
            _userRoles.RemoveRange(existedUserRoles);

            if (rolesIdList.Any())
            {
                var existRolesCount = await _roles.CountAsync(x => rolesIdList.Contains(x.Id));
                if (existRolesCount != rolesIdList.Count)
                {
                    throw new Exception("Таких ролей нету");
                }

                var userRolesToCreate = rolesIdList.Select(r => new UserRole
                {
                    UserId = userEntity.Id,
                    RoleId = r
                });
                await _userRoles.AddRangeAsync(userRolesToCreate);
            }

            await _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<ClaimEntity> GetClaimsForRoleAsync(int roleId)
        {
            var claimsForRole = _roleClaims.Where(rc => rc.RoleId == roleId)
                .Include(rc => rc.ClaimEntity)
                .Select(rc => rc.ClaimEntity)
                .AsAsyncEnumerable();

            return claimsForRole;
        }

        public async Task AddOrUpdateClaimsToRoleAsync(RoleClaimsUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var claimsIdList = model.ClaimIds?.ToList();
            if (claimsIdList == null)
            {
                throw new ArgumentNullException(nameof(model.ClaimIds));
            }

            var roleEntity = await _roles.FirstAsync(r => r.Id == model.RoleId);

            var existedRoleClaims = _roleClaims.Where(rc => rc.RoleId == roleEntity.Id);
            _roleClaims.RemoveRange(existedRoleClaims);

            if (claimsIdList.Any())
            {
                var existClaimsCount = await _claimEntities.CountAsync(x => claimsIdList.Contains(x.Id));
                if (existClaimsCount != claimsIdList.Count)
                {
                    throw new Exception("Таких клаймов нету");
                }

                var claimIdsToRoleClaim = claimsIdList.Select(c => new RoleClaim
                {
                    RoleId = roleEntity.Id,
                    ClaimEntityId = c
                });
                await _roleClaims.AddRangeAsync(claimIdsToRoleClaim);
            }

            await _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<ClaimEntity> GetClaimsForUserAsync(Guid userId)
        {
            var userClaims = _userClaims.Where(uc => uc.UserId == userId)
                .Include(uc => uc.ClaimEntity).ThenInclude(c => c.ClaimType)
                .Include(uc => uc.ClaimEntity).ThenInclude(c => c.ClaimValue)
                .Select(uc => uc.ClaimEntity)
                .AsAsyncEnumerable();

            return userClaims;
        }

        public async Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var claimsIdList = model.ClaimIds?.ToList();
            if (claimsIdList == null)
            {
                throw new Exception();
            }

            var userEntity = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (userEntity == null)
            {
                throw new Exception("EntityNotFoundException");
            }

            var existedUserClaims = _userClaims.Where(uc => uc.UserId == userEntity.Id);
            _userClaims.RemoveRange(existedUserClaims);

            if (claimsIdList.Any())
            {
                var existClaimsCount = await _claimEntities.CountAsync(uc => claimsIdList.Contains(uc.Id));
                if (existClaimsCount != claimsIdList.Count)
                {
                    throw new Exception("Таких клаймов нету");
                }

                var claimIdsToUserClaim = claimsIdList.Select(c => new UserClaim
                {
                    UserId = userEntity.Id,
                    ClaimEntityId = c
                });
                await _userClaims.AddRangeAsync(claimIdsToUserClaim);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
