using System;
using System.Linq;
using CustomIdentity.DAL.Converters;
using CustomIdentity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace CustomIdentity.DAL
{
    public class CustomIdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ClaimEntity> ClaimEntities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ClaimType> ClaimTypes { get; set; }
        public DbSet<ClaimValue> ClaimValues { get; set; }

        public CustomIdentityDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreatedAsync().GetAwaiter().GetResult();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaim>()
                .HasKey(uc => new {uc.UserClaimId, uc.UserId});
            
            var storeOptions = GetStoreOptions();
            var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
            var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
            PersonalDataConverter converter;

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).IsUnique();
                b.HasIndex(u => u.NormalizedEmail);
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                if (encryptPersonalData)
                {
                    converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                    var personalDataProps = typeof(User).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in personalDataProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException();
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }
            });
            
            modelBuilder.Entity<UserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                if (maxKeyLength > 0)
                {
                    b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
                }
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(ur => new
                {
                    ur.RoleId,
                    ur.UserId
                });
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.HasKey(ur => new
                {
                    ur.RoleId,
                    ur.UserClaimId
                });
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasIndex(r => r.Title).IsUnique();
            });

            modelBuilder.Entity<ClaimType>(b =>
            {
                b.HasIndex(r => r.Value).IsUnique();
            });

            modelBuilder.Entity<ClaimValue>(b =>
            {
                b.HasIndex(r => r.Value).IsUnique();
            });            
            
            modelBuilder.Entity<ClaimEntity>(b =>
            {
                b.HasIndex(r => new { r.ClaimTypeId, r.ClaimValueId }).IsUnique();
            });
        }

        private StoreOptions GetStoreOptions() => this.GetService<IDbContextOptions>()
            .Extensions.OfType<CoreOptionsExtension>()
            .FirstOrDefault()?.ApplicationServiceProvider
            ?.GetService<IOptions<IdentityOptions>>()
            ?.Value?.Stores;
    }
}
