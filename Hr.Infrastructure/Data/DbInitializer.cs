using Hr.Application.Common;
using Hr.Application.Common.Enums;
using Hr.Application.Common.Global;
using Hr.Application.Interfaces;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Hr.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async void Configure(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                await roleManager.SeedAdminRoleAsync();
                await userManager.SeedAdminUserAsync(roleManager);
            }
            
            // Other configuration code
        }
        public static async Task SeedAdminRoleAsync(this RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(SD.Roles.SuperAdmin.ToString()));
        }

        public static async Task SeedAdminUserAsync(this UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = new ApplicationUser
            {
                UserName = SD.AdminUserName,
                Email = SD.AdminUserName,
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(adminUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(adminUser,SD.AdminPasswoed);
               
                await userManager.AddToRoleAsync(adminUser, SD.Roles.SuperAdmin.ToString());
            }
            await roleManager.SeedClaimsToAdmin(adminUser);
        }

        public static async Task SeedClaimsToAdmin(this RoleManager<IdentityRole> roleManager, ApplicationUser adminUser)
        {
            var adminRole = await roleManager.FindByNameAsync(SD.Roles.SuperAdmin.ToString());
            if (adminRole != null)
            {
                foreach (Modules module in Enum.GetValues(typeof(Modules)))
                {
                    var allPermissions = Permission.GeneratePermissionList(module.ToString());
                    var allClaims = await roleManager.GetClaimsAsync(adminRole);
                    foreach (var permission in allPermissions)
                    {
                        if (!allClaims.Any(c => c.Type == SD.PermissionType && c.Value == permission))
                            await roleManager.AddClaimAsync(adminRole, new Claim(SD.PermissionType, permission));
                    }
                }
            }
        }

    }


   
}
