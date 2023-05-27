using System.Text.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Natter.Api.Filters;

public class ExceptionHandlerFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var excp = context.Exception;
        context.Result = new JsonResult(new { ErrorMessage = excp.Message })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
        };
    }
}