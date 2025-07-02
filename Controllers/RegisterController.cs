using Microsoft.AspNetCore.Mvc;
using Identity_User_Roles.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Identity_User_Roles.Controllers
{
    public class RegisterController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public RegisterController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(RegisterViewModel model)
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
                    var roleExist = await _roleManager.RoleExistsAsync("Student");

                    if (!roleExist)
                    {
                        var role = new IdentityRole("Student");
                        await _roleManager.CreateAsync(role);
                    }

                    await _userManager.AddToRoleAsync(user, "Student");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login","Account");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }



    }
}
