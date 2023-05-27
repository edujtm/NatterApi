using Microsoft.Extensions.Options;
using AspNetCoreRateLimit;

namespace Natter.Api.Middleware;

public class CustomIpRateLimiterMiddleware : RateLimitMiddleware<IpRateLimitProcessor>
{
    private ILogger<CustomIpRateLimiterMiddleware> _logger;
    private RateLimitOptions _options;

    public CustomIpRateLimiterMiddleware(
        RequestDelegate next,
        IOptions<IpRateLimitOptions> options,
        IProcessingStrategy processingStrategy,
        IIpPolicyStore policyStore,
        IRateLimitConfiguration configuration,
        ILogger<CustomIpRateLimiterMiddleware> logger
    ) : base(next, options.Value, new IpRateLimitProcessor(options.Value, policyStore, processingStrategy), configuration)
    {
        _options = options.Value;
        _logger = logger;
    }

    public override async Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
    {
        await httpContext.Response.WriteAsJsonAsync(new
        {
            ErrorMessage = _options.QuotaExceededMessage ?? "Request quota limit has been exceeded!",
        });
    }

    protected override void LogBlockedRequest(HttpContext httpContext, ClientRequestIdentity identity, RateLimitCounter counter, RateLimitRule rule)
    {
        _logger.LogInformation(
            "Blocked request from client {ClientIp}, quota {RequestQuantity}/{LimitPeriod} exceeded by {ExceededRequest}!",
            identity.ClientIp,
            rule.Limit,
            rule.Period,
            counter.Count - rule.Limit
        );
    }
}