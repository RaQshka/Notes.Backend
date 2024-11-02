using FluentValidation;
using Notes.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;
namespace Notes.WebApi.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException ex:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(ex.Errors);
                    break;
                case NotFoundException ex:
                    code = HttpStatusCode.NotFound;
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }
            return context.Response.WriteAsync(result);
        }

    }
}
