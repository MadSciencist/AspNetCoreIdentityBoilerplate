using AspNetCoreIdentityBoilerplate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspNetCoreIdentityBoilerplate.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        public static async Task SeedRolesAndUser(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                if (!await roleManager.RoleExistsAsync("admin") && !await roleManager.RoleExistsAsync("user"))
                {
                    await roleManager.CreateAsync(new AppRole("admin"));
                    await roleManager.CreateAsync(new AppRole("user"));
                }

                if (await userManager.FindByNameAsync("admin") != null) return;

                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                const string password = "admin1";

                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "admin");
            }
        }
    }
}
