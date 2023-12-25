using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace OnePhp.HRIS.Core.Data
{
    public class RoleDefiner
    {
        public static void SeedRoles(IServiceScope scope)
        {
            var userManager = (UserManager<ApplicationUser>)scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
            var roleManager = (RoleManager<ApplicationRole>)scope.ServiceProvider.GetService(typeof(RoleManager<ApplicationRole>));

            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("emp@fredley.group").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "emp@fredley.group";
                user.Email = "emp@fredley.group";
                user.FirstName = "Fredley";
                user.LastName = "Employee";

                IdentityResult result = userManager.CreateAsync(user, "Fredley1234!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
            if (userManager.FindByEmailAsync("admin@fredley.group").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin@fredley.group";
                user.Email = "admin@fredley.group";
                user.FirstName = "Admin Fredley ";
                user.LastName = "Group";
                IdentityResult result = userManager.CreateAsync(user, "Fredly1234!").Result;
                if (result.Succeeded)
                {   
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Employee").Result)
             {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Employee";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("HR Checker").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "HR Checker";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("HR Processor").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "HR Processor";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("HR Approver").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "HR Approver";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Payroll Processor").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Payroll Processor";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Payroll Approver").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Payroll Approver";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
