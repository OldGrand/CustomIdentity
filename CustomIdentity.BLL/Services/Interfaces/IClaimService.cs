﻿using System;
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
        Task AddOrUpdateUserClaimsAsync(UserClaimUpdateModel model);//TODO ToPermissionService
        Task CreateClaimAsync(ClaimCreateModel model);
        IAsyncEnumerable<ClaimEntity> GetAllClaimsAsync();
        Task DeleteClaimAsync(int claimId);
        IAsyncEnumerable<UserClaim> GetClaimsForUserAsync(Guid userId);
    }
}