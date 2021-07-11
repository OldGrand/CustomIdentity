using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class ClaimService : IClaimService
    {
        private readonly CustomIdentityDbContext _dbContext;//TODO Replace with repos and uow
        private readonly DbSet<UserClaim> _userClaims;

        public ClaimService(CustomIdentityDbContext dbContext)
        {
            _dbContext = dbContext;

            _userClaims = _dbContext.UserClaims;
        }

        public ValueTask<EntityEntry<UserClaim>> CreateClaimAsync(string type, string value)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var newClaim = new UserClaim
            {
                ClaimType = type,
                ClaimValue = value
            };
            
            return _userClaims.AddAsync(newClaim);
        }

        public Task CreateClaimAsync(Claim claim)
        {
            var claimsCollection = new[] { claim };
            return CreateRangeClaimsAsync(claimsCollection);
        }

        public Task CreateRangeClaimsAsync(IEnumerable<Claim> claims)
        {
            if (claims == null || !claims.Any())
            {
                throw new ArgumentNullException();
            }

            var newClaims = claims.Select(claim => new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });

            return _userClaims.AddRangeAsync(newClaims);
        }

        public void RemoveClaim(Claim claim)
        {
            var claimsCollection = new[] {claim};
            RemoveRangeClaims(claimsCollection);
        }

        public void RemoveRangeClaims(IEnumerable<Claim> claims)
        {
            if (claims == null || !claims.Any())
            {
                throw new ArgumentNullException();
            }

            var containClaims = _userClaims.Where(uc => claims.Any(x => x.Type == uc.ClaimType && x.Value == uc.ClaimValue));
            _userClaims.RemoveRange(containClaims);
        }
    }
}
