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
        IAsyncEnumerable<ClaimType> GetAllClaimTypesAsync();
        Task CreateClaimTypeAsync(string claimTypeValue);
        Task DeleteClaimTypeAsync(string claimType);
        IAsyncEnumerable<ClaimValue> GetAllClaimValuesAsync();
        Task CreateClaimValueAsync(string claimValue);
        Task DeleteClaimValueAsync(string claimValue);
        Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model);
        Task CreateClaimAsync(ClaimCreateModel model);
        IAsyncEnumerable<ClaimEntity> GetAllClaimsAsync();
        Task DeleteClaimAsync(int claimId);
        Task<IEnumerable<Claim>> GetClaimsForUserAsync(Guid userId);
    }
}