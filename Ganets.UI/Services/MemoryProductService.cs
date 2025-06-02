using Ganets.Domain.Entities.Models;
using Ganets.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ganets.UI.Services
{
    public class MemoryProductService : IProductService
    {
            List<Gadget> _gadgets;
            List<Category> _categories;
            IConfiguration _config;

        public MemoryProductService(ICategoryService categoryService, IConfiguration config)
            {
                _categories = categoryService.GetCategoryListAsync().Result.Data;
                _config = config;
                SetupData();
            } 
 
            /// <summary> 
            /// Инициализация списков 
            /// </summary> 
            private void SetupData()
            {
                _gadgets = new List<Gadget>
            {
                new Gadget {Id = 1, Name="IPhone 16 pro max",
                    Description="Дорогая фигня, не отличается от прошлых",
                    Price =3999, Image="Images/iphone.jpg",
                    
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("phones")).Id},
                    
                new Gadget { Id = 2, Name="Samsung s25",
                    Description="Норм телефон, если бы не цена и батарея",
                    Price =2700, Image="Images/samsung.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("phones")).Id},
                new Gadget { Id = 3, Name="Apple Watch s10",
                    Description="Норм часы, если бы не цена и батарея",
                    Price =1900, Image="Images/applewatch.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("watches")).Id},
                new Gadget { Id = 4, Name="Samsung Watch Ultra",
                    Description="Ни дизайна, ни батареии и цена космос",
                    Price =1899, Image="Images/samsungwatch.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("watches")).Id},
                new Gadget { Id = 5, Name="Плойка 5-ая",
                    Description="Если есть свободное время поможет его скоротать",
                    Price =2400, Image="Images/ps5.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("consoles")).Id}
            };
            }

        public Task<ResponseData<ListModel<Gadget>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Получение ID категории для фильтрации
            int? categoryId = null;
            if (categoryNormalizedName != null)
            {
                categoryId = _categories.FirstOrDefault(c => c.NormalizedName.Equals(categoryNormalizedName))?.Id;
            }

            // Фильтрация данных
            var data = _gadgets.Where(g => categoryId == null || g.CategoryId == categoryId).ToList();

            // Получение размера страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();

            // Вычисление общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

            // Получение данных текущей страницы
            var listData = new ListModel<Gadget>()
            {
                Items = data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // Поместить данные в результат
            var result = new ResponseData<ListModel<Gadget>>()
            {
                Data = listData
            };

            // Если список данных пуст
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }

            // Вернуть результат
            return Task.FromResult(result);
        }

        public Task<ResponseData<Gadget>> CreateProductAsync(Gadget productt, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}

