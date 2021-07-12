using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly CustomIdentityDbContext _dbContext;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<UserRole> _userRoles;


        public RoleService(CustomIdentityDbContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roles = _dbContext.Roles;
            _userRoles = _dbContext.UserRoles;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var newRoleEntity = new Role
            {
                Title = roleName
            };

            await _roles.AddAsync(newRoleEntity);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteRoleAsync(int roleId)
        {
            var roleToArray = new[] { roleId };
            return DeleteRolesRangeAsync(roleToArray);
        }

        public async Task DeleteRolesRangeAsync(IEnumerable<int> roleIds)
        {
            var existedRoles = await _roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();

            if (existedRoles.Count != roleIds.Count())
            {
                throw new Exception("чего-то не хватает");
            }

            _roles.RemoveRange(existedRoles);
            await _dbContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<Role> GetAllRolesAsync()
        {
            var roleEntities = _roles.AsAsyncEnumerable();
            return roleEntities;
        }

        public Task<Role> GetRoleAsync(int roleId)
        {
            var taskRoleEntity = _roles.FirstAsync(r => r.Id == roleId);
            return taskRoleEntity;
        }
        
        public IAsyncEnumerable<Role> GetRolesForUser(Guid userId)
        {
            var userWithIncludedRoles = _userRoles.Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .AsAsyncEnumerable();

            return userWithIncludedRoles;
        }
    }
}
