using Microsoft.AspNetCore.Mvc;

namespace Identity_User_Roles.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
