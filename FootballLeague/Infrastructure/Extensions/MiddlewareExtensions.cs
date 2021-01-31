namespace FootballLeague.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Middleware;

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
