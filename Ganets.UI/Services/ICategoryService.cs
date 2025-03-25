using Ganets.Domain.Entities.Models;
using Ganets.Domain.Entities;

namespace Ganets.UI.Services;

public interface ICategoryService
{
    /// <summary> 
    /// Получение списка всех категорий 
    /// </summary> 
    /// <returns></returns> 
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
}
