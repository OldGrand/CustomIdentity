using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly DbSet<ClaimEntity> _userClaims;
        private readonly DbSet<ClaimType> _claimTypes;
        private readonly DbSet<ClaimValue> _claimValues;
        private readonly UserManager<User> _userManager;

        public ClaimService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userClaims = _dbContext.ClaimEntities;
            _claimTypes = _dbContext.ClaimTypes;
            _claimValues = _dbContext.ClaimValues;
        }

        public async Task CreateClaimType(string claimTypeValue)
        {
            if (string.IsNullOrEmpty(claimTypeValue))
            {
                throw new ArgumentNullException(nameof(claimTypeValue));
            }

            var isClaimTypeAlreadyExist = await _claimTypes.AnyAsync(ct => ct.Value == claimTypeValue);
            if (isClaimTypeAlreadyExist)
            {
                throw new Exception("Value already exist");
            }

            var newClaimTypeEntity = new ClaimType
            {
                Value = claimTypeValue
            };

            await _claimTypes.AddAsync(newClaimTypeEntity);
        }

        public async Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var claimsIdList = model.ClaimIds?.ToList();
            if (claimsIdList == null || !claimsIdList.Any())
            {
                throw new Exception();
            }

            var userEntity = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (userEntity == null)
            {
                throw new Exception("EntityNotFoundException");
            }

            var existedNewClaimIds = await _userClaims.Where(uc => claimsIdList.Contains(uc.Id))
                .Include(uc => uc.ClaimType)
                .Include(uc => uc.ClaimValue)
                .Select(uc => new Claim(uc.ClaimType.Value, uc.ClaimValue.Value))
                .ToListAsync();

            if (existedNewClaimIds.Count != claimsIdList.Count)
            {
                throw new Exception("Таких клаймов нету");
            }

            await _userManager.AddClaimsAsync(userEntity, existedNewClaimIds);
        }

        public async Task CreateClaimAsync(ClaimCreateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var claimTypeEntity = await _claimTypes.FirstAsync(ct => ct.Id == model.ClaimTypeId);
            var claimValueEntity = await _claimValues.FirstAsync(ct => ct.Id == model.ClaimValueId);
            
            var newUserClaimModel = new ClaimEntity
            {
                ClaimType = claimTypeEntity,
                ClaimValue = claimValueEntity
            };

            await _userClaims.AddAsync(newUserClaimModel);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteClaimAsync(int claimId)
        {
            var roleToArray = new[] { claimId };
            return DeleteClaimsRangeAsync(roleToArray);
        }

        public async Task DeleteClaimsRangeAsync(IEnumerable<int> claimIds)
        {
            var existedRoles = await _userClaims.Where(r => claimIds.Contains(r.Id))
                .ToListAsync();

            if (existedRoles.Count != claimIds.Count())
            {
                throw new Exception("чего-то не хватает");
            }

            _userClaims.RemoveRange(existedRoles);
            await _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<ClaimEntity> GetAllClaimsAsync()
        {
            var userClaimEntities = _userClaims.AsAsyncEnumerable();
            return userClaimEntities;
        }

        public Task<ClaimEntity> GetClaimAsync(int claimId)
        {
            var taskRoleEntity = _userClaims.FirstAsync(r => r.Id == claimId);
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
