using Identity_User_Roles.Data;
using Identity_User_Roles.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("db")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequiredLength = 3;

    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = false;
})
    
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
name:"default",
pattern:"{controller=Account}/{action=Login}/{id?}"
);


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedService.SeedRoles(roleManager);
}


app.Run();
