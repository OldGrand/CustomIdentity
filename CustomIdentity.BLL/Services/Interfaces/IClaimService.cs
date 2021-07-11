using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomIdentity.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IClaimService
    {
        ValueTask<EntityEntry<UserClaim>> CreateClaimAsync(string type, string value);
        Task CreateClaimAsync(Claim claim);
        Task CreateRangeClaimsAsync(IEnumerable<Claim> claims);
        void RemoveClaim(Claim claim);
        void RemoveRangeClaims(IEnumerable<Claim> claims);
    }
}