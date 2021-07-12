using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity.Controllers
{
    public class PermissionController : ApiControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IMapper mapper,
            IPermissionService permissionService) : base(mapper)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public IActionResult GetClaimsForUser(Guid userId)
        {
            return Ok(_permissionService.GetClaimsForUserAsync(userId));
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserClaims(UserClaimUpdateModel model)
        {
            await _permissionService.AddOrUpdateUserClaimsAsync(model);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserRoles(UserRolesUpdateModel updateModel)
        {
            await _permissionService.AddOrUpdateUserRolesAsync(updateModel);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeRolesClaims(RoleClaimsUpdateModel updateModel)
        {
            await _permissionService.AddOrUpdateClaimsToRoleAsync(updateModel);

            return Ok();
        }
    }
}
