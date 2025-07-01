using Microsoft.AspNetCore.Mvc;

namespace Identity_User_Roles.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
