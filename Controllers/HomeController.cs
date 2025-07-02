using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_User_Roles.Controllers
{
    public class HomeController : Controller
    {
        public readonly SignInManager<IdentityUser> _signInManager;
        public HomeController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }


        [Authorize(Roles = "Student")]
        public IActionResult Student()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
