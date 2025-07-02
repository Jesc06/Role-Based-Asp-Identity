using Identity_User_Roles.Data;
using Microsoft.AspNetCore.Identity;


namespace Identity_User_Roles.Services
{
    public class SeedService
    {

        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

            try
            {
                //Ensure database is ready
                logger.LogInformation("Ensuring the database is created.");
                await context.Database.EnsureCreatedAsync();


                //Add Roles
                logger.LogInformation("Seeding roles.");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "Student");


                //Add admin user
                logger.LogInformation("seeding admin user");
                var adminEmail = "josh@gmail.com";
                if(await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                    };

                    var result = await userManager.CreateAsync(adminUser, "123");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Assigning Admin role to the user.");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogError("failed to create admin user");
                    }

                }
            }

            catch(Exception ex)
            {
                logger.LogError("an error while seeding");
            }


        }



        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string rolename)
        {
            if(!await roleManager.RoleExistsAsync(rolename))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(rolename));
                if (!result.Succeeded)
                {
                    throw new Exception("failed to create role");
                }
            }
        }







    }
}
