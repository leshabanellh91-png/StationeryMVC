using Microsoft.AspNetCore.Mvc;

namespace StationeryMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
