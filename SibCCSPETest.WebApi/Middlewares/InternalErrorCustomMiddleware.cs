using System.Text.Json;

namespace SibCCSPETest.WebApi.Middlewares
{
    public class InternalErrorCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public InternalErrorCustomMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var customResponse = new
                {
                    Code = 500,
                    Message = "Internal Server Error Occurred",
                    ExceptionDetails = ex.Message
                };
                var customJson = JsonSerializer.Serialize(customResponse);
                await context.Response.WriteAsync(customJson);
            }
        }
    }
}
