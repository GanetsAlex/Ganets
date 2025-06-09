using Serilog;

namespace Ganets.UI.Middleware
{
    public class FileLogger
    {
        private readonly RequestDelegate _next;

        public FileLogger(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext); // Передаем запрос дальше по конвейеру
            var code = httpContext.Response.StatusCode;

            if (code / 100 != 2) // Логируем только ошибки (коды, не начинающиеся на 2XX)
            {
                Log.Information($"---> Request {httpContext.Request.Path} returns {code}");
            }
        }
    }
}
