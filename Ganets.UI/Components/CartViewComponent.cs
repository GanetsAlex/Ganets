using Microsoft.AspNetCore.Mvc;

namespace Ganets.UI.Components
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
