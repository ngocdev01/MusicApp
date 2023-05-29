using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Domain.Common.Errors;

namespace MusicApp.Api.Common.Errors;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException httpResponseException)
        {
            context.Result = new ObjectResult(new {error = httpResponseException.Value })
            {
                StatusCode =(int)httpResponseException.StatusCode,
            };

            context.ExceptionHandled = true;
        }
        else if (context.Exception is Exception ex)
        {
            context.Result = new ObjectResult(new { error = ex.Message })
            {
                StatusCode = 500,
            };
        }
    }
}
