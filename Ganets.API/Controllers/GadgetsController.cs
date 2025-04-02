using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ganets.API.Data;
using Ganets.Domain.Entities;
using Ganets.Domain.Entities.Models;

namespace Ganets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GadgetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GadgetsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Gadgets
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Gadget>>>> GetGadgets(
     string? category, int pageNo = 1, int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Gadget>>();

            // Фильтрация по категории и загрузка данных категорий
            var data = _context.Gadgets
                .Include(g => g.Category)
                .Where(g => string.IsNullOrEmpty(category) || g.Category.NormalizedName.Equals(category));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);

            if (pageNo > totalPages)
                pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<Gadget>()
            {
                Items = await data
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // Поместить данные в объект результата
            result.Data = listData;

            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }

            return result;
        }


        // GET: api/Gadgets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gadget>> GetGadget(int id)
        {
            var gadget = await _context.Gadgets.FindAsync(id);

            if (gadget == null)
            {
                return NotFound();
            }

            return gadget;
        }

        // PUT: api/Gadgets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGadget(int id, Gadget gadget)
        {
            if (id != gadget.Id)
            {
                return BadRequest();
            }

            _context.Entry(gadget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GadgetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Gadgets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gadget>> PostGadget(Gadget gadget)
        {
            _context.Gadgets.Add(gadget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGadget", new { id = gadget.Id }, gadget);
        }

        // DELETE: api/Gadgets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGadget(int id)
        {
            var gadget = await _context.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return NotFound();
            }

            _context.Gadgets.Remove(gadget);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GadgetExists(int id)
        {
            return _context.Gadgets.Any(e => e.Id == id);
        }
    }
}
