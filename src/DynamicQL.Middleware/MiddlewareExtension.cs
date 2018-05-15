using Microsoft.AspNetCore.Builder;

namespace DynamicQL.Middleware
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseDynamicQL(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DynamicQLMiddleware>();
        }
    }
}
