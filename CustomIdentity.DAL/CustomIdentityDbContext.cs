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
        public DbSet<UserClaimAssociative> UserClaimAssociatives { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        public CustomIdentityDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreatedAsync().GetAwaiter().GetResult();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaimAssociative>()
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
                b.ToTable("AspNetUsers");
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

                b.ToTable("AspNetUserLogins");
            });
        }

        private StoreOptions GetStoreOptions() => this.GetService<IDbContextOptions>()
            .Extensions.OfType<CoreOptionsExtension>()
            .FirstOrDefault()?.ApplicationServiceProvider
            ?.GetService<IOptions<IdentityOptions>>()
            ?.Value?.Stores;
    }
}
