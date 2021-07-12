using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IClaimService
    {
        Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model);
        Task CreateClaimAsync(ClaimCreateModel model);
        IAsyncEnumerable<ClaimEntity> GetAllClaimsAsync();
        Task DeleteClaimAsync(int claimId);
        Task<IEnumerable<Claim>> GetClaimsForUserAsync(Guid userId);
    }
}