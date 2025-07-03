# Role Based Set up Configuration

### 1. Create a folder named Services in your project, and add a class file named SeedServices

```csharp

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


```

<br>

### 2. Register the SeedServices class in Program.cs

```csharp

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();

```


<br>

```csharp

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedService.SeedRoles(roleManager);
}

```
<br>


Make sure it is placed above app.Run() at the bottom of Program.cs


<br>



### 3. After registering the service in Program.cs, create a controller for account registration and assign a role to the user.

```csharp

        [HttpPost]
        public async Task<ActionResult> RegisterAccount(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = model.email,
                    UserName = model.email
                };

                var result = await _userManager.CreateAsync(user,model.password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.role);

                    return RedirectToAction("Login","Account");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }


```


<br>

### 4. Create a controller that handles user login functionality.

```csharp

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.email, model.password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.email);

                    if(await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home2");
                    }
               
                }
            }
            return View();
        }


```

<br>

### Use the [Authorize] attribute to authenticate and authorize access to the page based on roles

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_User_Roles.Controllers
{
    [Authorize(Roles = "User")]
    public class Home2Controller : Controller
    {


        private readonly SignInManager<IdentityUser> _signInManager;
        public Home2Controller(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



    }
}


```

<br>

### This is the method where, once the user is authenticated, they won't be able to go back to the login page using the browser's back button unless they log out first. This is implemented by checking the user's current role.

<br>

```csharp
      public async Task<IActionResult> Login()
      {
          if (User.Identity.IsAuthenticated)
          {
              var Authenticated_Username = User.Identity.Name;//find current username authenticated user
              var user = await _userManager.FindByEmailAsync(Authenticated_Username);
              if(await _userManager.IsInRoleAsync(user, "Admin"))
              {
                  return RedirectToAction("Index", "Home");
              }
              else
              {
                  return RedirectToAction("Index", "Home2");
              }   
          }
          return View();
      }

```



<br>
<br>
<br>
<br>
<br>
<br>



# This is the method to use if you don't want to allow admin registration through the UI. Instead, you can hardcode the admin's email and password in the code using default seeding.

<br>

### Modify the SeedService

<br>

```csharp
using Microsoft.AspNetCore.Identity;

namespace Identity_User_Roles.Services
{
    public class SeedService
    {
        public static async Task SeedRolesAndAdmin(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            string[] roles = { "Admin", "User" };

            // 1. Seed Roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Seed Admin User
            var adminEmail = "admin@example.com"; // CHANGE THIS
            var adminPassword = "Admin@123";      // CHANGE THIS

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
                    // Optional: Log error or throw exception if creation failed
                    throw new Exception("Failed to create admin user.");
                }
            }
        }
    }
}


```

<br>


### Register the new modified service in program.cs

<br>

```csharp

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    await SeedService.SeedRolesAndAdmin(roleManager, userManager);
}


```

