using localsound.backend.Domain.Model.Exception;
using Microsoft.AspNetCore.Antiforgery;
using System.Net;
using System.Text.Json;

namespace localsound.backend.api.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AntiforgeryValidationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);

                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            var statusCode = GetStatusCode(e);

            var response = new
            {
                title = GetTitle(e),
                status = statusCode,
                detail = e.Message,
                errors = GetErrors(e)
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private IReadOnlyDictionary<string,string[]> GetErrors(Exception e)
        {
            if (e is ValidatorException validationException)
            {
                IReadOnlyDictionary<string, string[]> errors = validationException.Errors;
                return errors;
            }
            return new Dictionary<string, string[]>();
        }

        private string GetTitle(Exception e) =>
            e switch
            {
                ValidatorException => "ValidationException",
                _ => "ServerError"
            };

        private int GetStatusCode(Exception e) => 
            e switch
        {
            ValidatorException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
