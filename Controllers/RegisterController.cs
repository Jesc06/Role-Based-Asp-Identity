using Microsoft.AspNetCore.Mvc;
using Identity_User_Roles.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Identity_User_Roles.Controllers
{
    public class RegisterController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RegisterController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
