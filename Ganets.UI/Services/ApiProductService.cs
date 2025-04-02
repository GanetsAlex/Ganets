using Ganets.Domain.Entities;
using Ganets.Domain.Entities.Models;

namespace Ganets.UI.Services
{
    public class ApiProductService(HttpClient httpClient) : IProductService
    {

        public async Task<ResponseData<ListModel<Gadget>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;

            // Формирование данных для строки запроса
            var queryData = new Dictionary<string, string>
        {
            { "pageNo", pageNo.ToString() }
        };

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName);
            }

            var query = QueryString.Create(queryData);

            // Выполнение HTTP-запроса
            var result = await httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Gadget>>>();
            }

            // Формирование объекта ответа с ошибкой
            var response = new ResponseData<ListModel<Gadget>>
            {
                Success = false,
                ErrorMessage = "Ошибка чтения API"
            };
            return response;
        }
    }

}
