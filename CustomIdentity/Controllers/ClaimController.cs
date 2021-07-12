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

        [HttpGet]
        public IActionResult GetAllClaimTypes()
        {
            return Ok(_claimService.GetAllClaimTypesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaimType(string claimType)
        {
            await _claimService.CreateClaimTypeAsync(claimType);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClaimType(string claimType)
        {
            await _claimService.DeleteClaimTypeAsync(claimType);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllClaimValues()
        {
            return Ok(_claimService.GetAllClaimValuesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaimValue(string claimValue)
        {
            await _claimService.CreateClaimValueAsync(claimValue);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClaimValue(string claimValue)
        {
            await _claimService.DeleteClaimValueAsync(claimValue);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaim(ClaimCreateModel model)
        {
            await _claimService.CreateClaimAsync(model);

            return Ok();
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
    }
}
