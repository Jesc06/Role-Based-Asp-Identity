using Microsoft.AspNetCore.Mvc;

namespace Identity_User_Roles.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
