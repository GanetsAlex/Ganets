using Ganets.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ganets.UI.Controllers
{
    public class ImageController(UserManager<ApplicationUser>userManager) : Controller
    {
        public async Task<IActionResult> GetAvatar()
        {
            //==== Чтобы браузер не кешировал аватар ======
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            //=============================================
            var email = User.FindFirst(ClaimTypes.Email)!.Value;
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Avatar != null)
                return File(user.Avatar, user.MimeType);

            var imagePath = Path.Combine("Images", "ava.jpg"); 
            return File(imagePath, "image/jpg");
        }
    }
}
