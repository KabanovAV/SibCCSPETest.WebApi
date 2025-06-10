using Serilog;
using System.Diagnostics;

namespace SibCCSPETest.WebApi.Middlewares
{
    public class LoggingCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingCustomMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            Log.Information($"Начало обработки запроса: {context.Request.Path}");

            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();

            Log.Information($"Запрос {context.Request.Path} обработан за {sw.ElapsedMilliseconds} мс");
        }
    }
}
