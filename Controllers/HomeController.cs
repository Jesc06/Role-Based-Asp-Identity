using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Identity_User_Roles.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Account");
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
