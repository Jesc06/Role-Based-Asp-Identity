using Identity_User_Roles.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity_User_Roles.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
 
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


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




    }
}
