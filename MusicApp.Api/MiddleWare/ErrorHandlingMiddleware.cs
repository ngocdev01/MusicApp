using MusicApp.Domain.Common.Errors;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicApp.Api.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }

        }

        private Task HandleException(HttpContext context, Exception ex)
        {

            context.Response.ContentType = "application/json";

            if (ex is HttpResponseException exception)
            {
                var code = exception.StatusCode;
                var message = exception.Value;
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new
                        {
                            statusCode = code,
                            message = message,
                        }));
            }
            else return context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new
                        {
                            
                            message = ex.Message,
                            exeption = ex.InnerException,
                        })); ;
           
        }
    }
}
