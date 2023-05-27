

namespace Natter.Api.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next) => this._next = next;

    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(state =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "0");
            context.Response.Headers.Add("Cache-Control", "no-store");
            context.Response.Headers.Add("Content-Security-Policy",
                "default-src 'none'; frame-ancestors 'none'; sandbox");
            context.Response.Headers.Add("Server", "");
            return Task.FromResult(0);
        }, context);

        await this._next(context);
    }
}
