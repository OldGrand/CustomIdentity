using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomIdentity.BLL.Infrastructure.Comparers;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class ClaimService : IClaimService
    {
        private readonly CustomIdentityDbContext _dbContext;//TODO Replace with repos and uow
        private readonly DbSet<ClaimEntity> _claimEntities;
        private readonly DbSet<ClaimType> _claimTypes;
        private readonly DbSet<ClaimValue> _claimValues;
        private readonly DbSet<UserClaim> _userClaims;
        private readonly UserManager<User> _userManager;

        public ClaimService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userClaims = _dbContext.UserClaims;
            _claimTypes = _dbContext.ClaimTypes;
            _claimValues = _dbContext.ClaimValues;
            _claimEntities = _dbContext.ClaimEntities;
        }

        public IAsyncEnumerable<ClaimType> GetAllClaimTypesAsync()
        {
            var allClaimTypes = _claimTypes.AsAsyncEnumerable();
            return allClaimTypes;
        }

        public async Task CreateClaimTypeAsync(string claimType)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }
            
            var newClaimTypeEntity = new ClaimType
            {
                Value = claimType
            };

            await _claimTypes.AddAsync(newClaimTypeEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteClaimTypeAsync(string claimType)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            var claimTypeEntity = await _claimTypes.FirstAsync(ct => ct.Value == claimType);

            _claimTypes.Remove(claimTypeEntity);
            await _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<ClaimValue> GetAllClaimValuesAsync()
        {
            var allClaimValues = _claimValues.AsAsyncEnumerable();
            return allClaimValues;
        }

        public async Task CreateClaimValueAsync(string claimValue)
        {
            if (string.IsNullOrEmpty(claimValue))
            {
                throw new ArgumentNullException(nameof(claimValue));
            }

            var newClaimValueEntity = new ClaimValue
            {
                Value = claimValue
            };

            await _claimValues.AddAsync(newClaimValueEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteClaimValueAsync(string claimValue)
        {
            if (string.IsNullOrEmpty(claimValue))
            {
                throw new ArgumentNullException(nameof(claimValue));
            }

            var claimValueEntity = await _claimValues.FirstAsync(ct => ct.Value == claimValue);

            _claimValues.Remove(claimValueEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userEntity = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (userEntity == null)
            {
                throw new Exception("EntityNotFoundException");
            }

            var claimsIdList = model.ClaimIds?.ToList();
            if (claimsIdList == null || !claimsIdList.Any())
            {
                throw new Exception();
            }

            var isAllClaimIdsExist = await _userClaims.AllAsync(uc => claimsIdList.Contains(uc.ClaimEntityId));
            if (!isAllClaimIdsExist)
            {
                throw new Exception("Таких клаймов нету");
            }

            var existedUserClaims = await _userClaims.Where(uc => uc.UserId == userEntity.Id)
                .ToListAsync();
            var claimIdsToUserClaim = claimsIdList.Select(c => new UserClaim
            {
                UserId = userEntity.Id,
                ClaimEntityId = c
            }).ToList();

            var userClaimEqualityComparer = new UserClaimEqualityComparer();

            var claimsToDelete = existedUserClaims.Except(claimIdsToUserClaim, userClaimEqualityComparer);
            _userClaims.RemoveRange(claimsToDelete);

            var claimsToCreate = claimIdsToUserClaim.Except(existedUserClaims, userClaimEqualityComparer);
            await _userClaims.AddRangeAsync(claimsToCreate);
        }

        public async Task CreateClaimAsync(ClaimCreateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var newUserClaimModel = new ClaimEntity
            {
                ClaimTypeId = model.ClaimTypeId,
                ClaimValueId = model.ClaimValueId
            };

            await _claimEntities.AddAsync(newUserClaimModel);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteClaimAsync(int claimId)
        {
            var roleToArray = new[] { claimId };
            return DeleteClaimsRangeAsync(roleToArray);
        }

        public async Task DeleteClaimsRangeAsync(IEnumerable<int> claimIds)
        {
            var existedRoles = await _claimEntities.Where(r => claimIds.Contains(r.Id))
                .ToListAsync();

            if (existedRoles.Count != claimIds.Count())
            {
                throw new Exception("чего-то не хватает");
            }

            _claimEntities.RemoveRange(existedRoles);
            await _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<ClaimEntity> GetAllClaimsAsync()
        {
            var userClaimEntities = _claimEntities.AsAsyncEnumerable();
            return userClaimEntities;
        }

        public Task<ClaimEntity> GetClaimAsync(int claimId)
        {
            var taskRoleEntity = _claimEntities.FirstAsync(r => r.Id == claimId);
            return taskRoleEntity;
        }

        public async Task<IEnumerable<Claim>> GetClaimsForUserAsync(Guid userId)
        {
            var userEntity = await _userManager.FindByIdAsync(userId.ToString());

            var userClaims = await _userManager.GetClaimsAsync(userEntity);
            return userClaims;
        }
    }
}
