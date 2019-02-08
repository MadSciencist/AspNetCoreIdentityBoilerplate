using System;
using System.Text;
using AspNetCoreIdentityBoilerplate.Infrastructure;
using AspNetCoreIdentityBoilerplate.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreIdentityBoilerplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddAuthentication(services);
            services.AddTransient<ITokenBuilder, TokenBuilder>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionStrings:MsSql"];

            services.AddDbContext<AppIdentityDbContext>(options => { options.UseSqlServer(connectionString); });

            services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 5; // Sample validator
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        SaveSigninToken = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["AuthenticationJwt:Issuer"],
                        ValidAudience = Configuration["AuthenticationJwt:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthenticationJwt:Key"])),
                        ClockSkew = TimeSpan.FromMinutes(0)
                    });

            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();


            AppIdentityDbContext.SeedRolesAndUser(app.ApplicationServices).Wait();
        }
    }
}
