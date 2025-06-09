using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ganets.Domain.Entities;
using Ganets.UI.Services;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Ganets.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public EditModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [BindProperty]
        public Gadget gadget { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var result = await _productService.GetProductByIdAsync(id.Value);
            if (!result.Success || result.Data == null) return NotFound();

            gadget = result.Data;

            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _productService.UpdateProductAsync(gadget.Id, gadget, Image);
            return RedirectToPage("./Index");
        }
    }
}
