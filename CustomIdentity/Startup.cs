using System;
using System.Text;
using CustomIdentity.BLL.AutoMapper;
using CustomIdentity.BLL.Constants;
using CustomIdentity.BLL.Services.Implementation;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CustomIdentity.DAL.Entities;
using CustomIdentity.DAL.Stores;
using CustomIdentity.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CustomIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var keyBytes = Encoding.UTF8.GetBytes(Configuration[TokenConstants.JwtSecret]);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = securityKey,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization();

            services.AddCustomIdentity<User>(option =>
                {
                    option.Stores.ProtectPersonalData = true;
                }).AddDefaultTokenProviders();
            services.AddTransient<IUserStore<User>, CustomUserStore>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddAutoMapper(typeof(BusinessLogicLayerModelsProfile));

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomIdentity", Version = "v1" });
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomIdentity v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
