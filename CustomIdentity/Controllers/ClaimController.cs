using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity.Controllers
{
    public class ClaimController : ApiControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IMapper mapper,
            IClaimService claimService) : base(mapper)
        {
            _claimService = claimService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaim(UserClaimCreateModel model)
        {
            await _claimService.CreateClaimAsync(model);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetClaimsForUser(Guid userId)
        {
            var userClaims = await _claimService.GetClaimsForUserAsync(userId);

            return Ok(userClaims);
        }

        [HttpGet]
        public IActionResult GetAllClaims()
        {
            return Ok(_claimService.GetAllClaimsAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClaim(int claimId)
        {
            await _claimService.DeleteClaimAsync(claimId);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserClaims(UserClaimAssociativesUpdateModel model)
        {
            await _claimService.AddOrUpdateUserClaimsAsync(model);

            return Ok();
        }
    }
}
