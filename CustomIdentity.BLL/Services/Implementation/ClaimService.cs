﻿using System;
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

        public Task DeleteClaimTypeAsync(int claimTypeId)
        {
            var claimTypeToArray = new[] { claimTypeId };
            return DeleteClaimTypeRangeAsync(claimTypeToArray);
        }

        public async Task DeleteClaimTypeRangeAsync(IEnumerable<int> claimTypeIds)
        {
            var existedClaimTypes = await _claimTypes.Where(r => claimTypeIds.Contains(r.Id))
                .ToListAsync();

            if (existedClaimTypes.Count != claimTypeIds.Count())
            {
                throw new Exception("чего-то не хватает");
            }

            _claimTypes.RemoveRange(existedClaimTypes);
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

        public Task DeleteClaimValueAsync(int claimValueId)
        {
            var claimValueToArray = new[] { claimValueId };
            return DeleteClaimValueRangeAsync(claimValueToArray);
        }

        public async Task DeleteClaimValueRangeAsync(IEnumerable<int> claimValueIds)
        {
            var existedClaimValues = await _claimValues.Where(r => claimValueIds.Contains(r.Id))
                .ToListAsync();

            if (existedClaimValues.Count != claimValueIds.Count())
            {
                throw new Exception("чего-то не хватает");
            }

            _claimValues.RemoveRange(existedClaimValues);
            await _dbContext.SaveChangesAsync();
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
    }
}
