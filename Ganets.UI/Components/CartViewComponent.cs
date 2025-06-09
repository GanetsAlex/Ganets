using Ganets.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ganets.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }
    }

}
