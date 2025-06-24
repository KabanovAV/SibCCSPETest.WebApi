namespace SibCCSPETest.WebApi.Middlewares
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingCustomMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<LoggingCustomMiddleware>();

        public static IApplicationBuilder InternalErrorCustomMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<InternalErrorCustomMiddleware>();

        public static IApplicationBuilder UseNotFoundCustomMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<NotFoundCustomMiddleware>();        
    }
}
