using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class PermissionService : IPermissionService
    {
        private readonly CustomIdentityDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IClaimService _claimService;
        private readonly IRoleService _roleService;

        private readonly DbSet<RoleClaim> _roleClaims;

        public PermissionService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager,
            IClaimService claimService,
            IRoleService roleService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _claimService = claimService;
            _roleService = roleService;

            _roleClaims = _dbContext.RoleClaims;
        }


    }
}
