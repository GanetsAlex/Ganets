using Microsoft.AspNetCore.Mvc;
using Ganets.UI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ganets.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly List<ListDemo> _listData;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _listData = new List<ListDemo>
                {
                    new ListDemo {Id=1, Name="Item 1"},
                    new ListDemo {Id=2, Name="Item 2"},
                    new ListDemo {Id=3, Name="Item 3"}
                };
        }
        public IActionResult Index()
        {
            {
                ViewData["Message"] = "Лабораторная работа №2";
            }

            // Передача данных в представление через SelectList
            ViewData["Lst"] = new SelectList(_listData, "Id", "Name");

            return View();
        }
    }
}
