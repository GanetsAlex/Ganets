using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ganets.Domain.Entities;
using Ganets.UI.Data;
using Ganets.UI.Services;

namespace Ganets.UI.Areas.Admin.Pages
{
    public class DetailsModel(IProductService productService) : PageModel
    {
        public Gadget gadget { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await productService.GetProductByIdAsync(id.Value);
            if (!result.Success || result.Data == null)
            {
                return NotFound();
            }

            gadget = result.Data;
            return Page();
        }
    }
}
