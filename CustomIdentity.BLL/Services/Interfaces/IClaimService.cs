using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IClaimService
    {
        IAsyncEnumerable<ClaimType> GetAllClaimTypesAsync();
        Task CreateClaimTypeAsync(string claimTypeValue);
        Task DeleteClaimTypeAsync(int claimTypeId);

        IAsyncEnumerable<ClaimValue> GetAllClaimValuesAsync();
        Task CreateClaimValueAsync(string claimValue);
        Task DeleteClaimValueAsync(int claimValueId);

        Task CreateClaimAsync(ClaimCreateModel model);
        IAsyncEnumerable<ClaimEntity> GetAllClaimsAsync();
        Task DeleteClaimAsync(int claimId);
    }
}