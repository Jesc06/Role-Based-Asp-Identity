using Microsoft.AspNetCore.Mvc;
using Identity_User_Roles.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Identity_User_Roles.Controllers
{
    public class RegisterController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public RegisterController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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




    }
}
