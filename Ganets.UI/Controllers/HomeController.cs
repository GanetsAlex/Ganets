using Microsoft.AspNetCore.Mvc;

namespace Ganets.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
