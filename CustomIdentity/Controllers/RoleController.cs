using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Models;
using CustomIdentity.BLL.Models.Permissions;
using CustomIdentity.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity.Controllers
{
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IMapper mapper,
            IRoleService roleService) : base(mapper)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            return Ok(_roleService.GetAllRolesAsync());
        }

        [HttpGet]
        public IActionResult GetRolesForUser(Guid userId)
        {
            return Ok(_roleService.GetRolesForUser(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            await _roleService.CreateRoleAsync(roleName);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId);

            return Ok();
        }
    }
}
