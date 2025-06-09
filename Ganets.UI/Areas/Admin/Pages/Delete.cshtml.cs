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
    public class DeleteModel(IProductService productService) : PageModel
    {
        public Gadget gadget { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await productService.GetProductByIdAsync(id);
            if (!result.Success || result.Data is null)
            {
                return RedirectToPage("./Index");
            }

            gadget = result.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await productService.DeleteProductAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
