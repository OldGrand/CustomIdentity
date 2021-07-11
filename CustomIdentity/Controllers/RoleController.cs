using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Extensions;
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
    }
}
