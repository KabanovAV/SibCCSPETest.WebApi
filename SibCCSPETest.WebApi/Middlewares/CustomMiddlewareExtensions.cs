namespace SibCCSPETest.WebApi.Middlewares
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseNotFoundCustomMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<NotFoundCustomMiddleware>();

        public static IApplicationBuilder UseLoggingCustomMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<LoggingCustomMiddleware>();
    }
}
