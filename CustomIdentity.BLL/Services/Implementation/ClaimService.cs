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
        private readonly DbSet<UserClaim> _userClaims;
        private readonly UserManager<User> _userManager;

        public ClaimService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userClaims = _dbContext.UserClaims;
        }

        public async Task AddOrUpdateUserClaimsAsync(UserClaimAssociativesUpdateModel model)
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

            var claimIdListToClaims = model.ClaimIds.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue));

            await _userManager.AddClaimsAsync(userEntity, claimIdListToClaims);
        }

        public async Task CreateClaimAsync(UserClaimCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.ClaimValue) || string.IsNullOrEmpty(model.ClaimType))
            {
                throw new ArgumentNullException(nameof(model));
            }

            var isThereSimilarClaim = await _userClaims.AnyAsync(uc => uc.ClaimType == model.ClaimType &&
                                                                 uc.ClaimValue == model.ClaimValue);

            if (isThereSimilarClaim)
            {
                throw new Exception(nameof(model));
            }

            var newUserClaimModel = new UserClaim
            {
                ClaimType = model.ClaimType,
                ClaimValue = model.ClaimValue
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

        public IAsyncEnumerable<UserClaim> GetAllClaimsAsync()
        {
            var userClaimEntities = _userClaims.AsAsyncEnumerable();
            return userClaimEntities;
        }

        public Task<UserClaim> GetClaimAsync(int claimId)
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
