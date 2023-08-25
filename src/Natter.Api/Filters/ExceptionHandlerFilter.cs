namespace Natter.Api.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionHandlerFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionHandlerFilter> _logger;

    public ExceptionHandlerFilter(ILogger<ExceptionHandlerFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var excp = context.Exception;
        _logger.LogError(excp, "Some error ocurred while processing the request.");

        context.Result = new JsonResult(new { ErrorMessage = excp.Message })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
        };
    }
}
