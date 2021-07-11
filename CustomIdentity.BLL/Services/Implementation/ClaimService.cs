using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task<IEnumerable<Claim>> GetClaimsForUser(Guid userId)
        {
            var userEntity = await _userManager.FindByIdAsync(userId.ToString());

            if (userEntity == null)
            {
                throw new ArgumentException("EntityNotFoundException");
            }

            var userClaims = await _userManager.GetClaimsAsync(userEntity);
            return userClaims;
        }
    }
}
