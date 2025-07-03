using Identity_User_Roles.Data;
using Microsoft.AspNetCore.Identity;


namespace Identity_User_Roles.Services
{
    public class SeedService
    {
        
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

    }
}
