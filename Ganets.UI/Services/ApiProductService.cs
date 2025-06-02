using Ganets.Domain.Entities;
using Ganets.Domain.Entities.Models;
using System.Text.Json;

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

        public async Task<ResponseData<Gadget>> CreateProductAsync(Gadget product, IFormFile?formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Подготовить объект, возвращаемый методом 
            var responseData = new ResponseData<Gadget>();

            // Послать запрос к API для сохранения объекта 
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress,
        product);

            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось создать объект: { response.StatusCode}"; 
                return responseData;            
            }
            // Если файл изображения передан клиентом 
            if (formFile != null)
            {
                // получить созданный объект из ответа Api-сервиса 
                var gadget = await response.Content.ReadFromJsonAsync<Gadget>();
                // создать объект запроса 
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}/{gadget.Id}")
                };

                // Создать контент типа multipart form-data 
                var content = new MultipartFormDataContent();
                // создать потоковый контент из переданного файла 
                var streamContent = new StreamContent(formFile.OpenReadStream());
                // добавить потоковый контент в общий контент по именем "image" 
                content.Add(streamContent, "image", formFile.FileName);
                // поместить контент в запрос 
                request.Content = content;
                // послать запрос к Api-сервису 
                response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение: { response.StatusCode}"; 
                }
            }
            return responseData;
        }
    }
}
