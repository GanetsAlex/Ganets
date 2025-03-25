using Ganets.Domain.Entities.Models;
using Ganets.Domain.Entities;

namespace Ganets.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>>
    GetCategoryListAsync()
        {
            var categories = new List<Category>
        {
            new Category {Id=1, Name="Телефоны", NormalizedName="phones"},
            new Category {Id=2, Name="Игровые приставки", NormalizedName="consoles"}, 
            new Category {Id=3, Name="Умные часы", NormalizedName="watches"}      
        };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
