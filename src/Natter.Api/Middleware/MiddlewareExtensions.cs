
namespace Natter.Api.Middleware;

public static class MiddlewareExtensions
{
    public static void UseSecurityHeaders(this IApplicationBuilder app) => app.UseMiddleware<SecurityHeadersMiddleware>();
}
