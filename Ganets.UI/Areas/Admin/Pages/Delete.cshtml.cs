using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ganets.Domain.Entities;
using Ganets.UI.Data;

namespace Ganets.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Ganets.UI.Data.DataDbContext _context;

        public DeleteModel(Ganets.UI.Data.DataDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Gadget Gadget { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets.FirstOrDefaultAsync(m => m.Id == id);

            if (gadget == null)
            {
                return NotFound();
            }
            else
            {
                Gadget = gadget;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gadget = await _context.Gadgets.FindAsync(id);
            if (gadget != null)
            {
                Gadget = gadget;
                _context.Gadgets.Remove(Gadget);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
