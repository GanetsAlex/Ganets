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
        private readonly IWebHostEnvironment _env;

        public GadgetsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        [HttpPost("{id}")]
        public async Task<IActionResult> SaveImage(int id, IFormFile image)
        {
            Console.WriteLine("Метод SaveImage вызван!");
            // Найти объект по Id 
            var gadget = await _context.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return NotFound();
            }

            // Путь к папке wwwroot/Images 
            var imagesPath = Path.Combine(_env.WebRootPath, "Images");
            // получить случайное имя файла 
            var randomName = Path.GetRandomFileName();
            // получить расширение в исходном файле 
            var extension = Path.GetExtension(image.FileName);
            // задать в новом имени расширение как в исходном файле 
            var fileName = Path.ChangeExtension(randomName, extension);
            // полный путь к файлу 
            var filePath = Path.Combine(imagesPath, fileName);
            // создать файл и открыть поток для записи 
            using var stream = System.IO.File.OpenWrite(filePath);
            // скопировать файл в поток 
            await image.CopyToAsync(stream);
            // получить Url хоста 
            var host = "https://" + Request.Host;
            // Url файла изображения 
            var url = $"{host}/Images/{fileName}";
            // Сохранить url файла в объекте 
            gadget.Image = url;
            Console.WriteLine($"Сохраненный URL: {url}");
            await _context.SaveChangesAsync();
            _context.Entry(gadget).Reload();
            return Ok();
        }


        [HttpPost("UpdateImage/{id}")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile image)
        {
            var gadget = await _context.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return NotFound();
            }

            // Получить путь к папке wwwroot/Images
            var imagesPath = Path.Combine(_env.WebRootPath, "Images");

            // Удалить старое изображение
            if (!string.IsNullOrEmpty(gadget.Image))
            {
                var oldImagePath = Path.Combine(imagesPath, Path.GetFileName(gadget.Image));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Создать новое изображение
            var randomName = Path.GetRandomFileName();
            var extension = Path.GetExtension(image.FileName);
            var fileName = Path.ChangeExtension(randomName, extension);
            var filePath = Path.Combine(imagesPath, fileName);

            using var stream = System.IO.File.OpenWrite(filePath);
            await image.CopyToAsync(stream);

            // Сохранить новый URL изображения
            var host = "https://" + Request.Host;
            gadget.Image = $"{host}/Images/{fileName}";
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGadget(int id)
        {
            var gadget = await _context.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return NotFound();
            }

            // Удалить изображение гаджета
            if (!string.IsNullOrEmpty(gadget.Image))
            {
                var imagePath = Path.Combine(_env.WebRootPath, "Images", Path.GetFileName(gadget.Image));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
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
