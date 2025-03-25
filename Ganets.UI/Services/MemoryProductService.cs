using Ganets.Domain.Entities.Models;
using Ganets.Domain.Entities;

namespace Ganets.UI.Services
{
    public class MemoryProductService : IProductService
    {
            List<Gadget> _gadgets;
            List<Category> _categories;

            public MemoryProductService(ICategoryService categoryService)
            {
                _categories = categoryService.GetCategoryListAsync().Result.Data;
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
            // Найти ID категории для фильтрации
            int? categoryId = null;
            if (categoryNormalizedName != null)
            {
                categoryId = _categories.FirstOrDefault(c => c.NormalizedName.Equals(categoryNormalizedName))?.Id;
            }

            // Фильтровать объекты по категории
            var filteredGadgets = _gadgets.Where(g => categoryId == null || g.CategoryId == categoryId).ToList();

            var result = new ResponseData<ListModel<Gadget>>
            {
                Data = new ListModel<Gadget>
                {
                    Items = filteredGadgets,
                    CurrentPage = pageNo,
                    TotalPages = (int)Math.Ceiling(filteredGadgets.Count / 10.0)
                }
            };

            if (!filteredGadgets.Any())
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории.";
            }

            return Task.FromResult(result);
        }

    }
}

