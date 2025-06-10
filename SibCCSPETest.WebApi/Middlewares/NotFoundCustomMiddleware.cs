using System.Text.Json;

namespace SibCCSPETest.WebApi.Middlewares
{
    public class NotFoundCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundCustomMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var customResponse = new
                {
                    Code = 404,
                    Message = "Endpoint does not exist"
                };
                var customJson = JsonSerializer.Serialize(customResponse);
                await context.Response.WriteAsync(customJson);
            }
        }
    }
}
