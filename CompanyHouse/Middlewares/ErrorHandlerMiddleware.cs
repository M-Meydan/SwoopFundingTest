using API.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory logger) { _next = next; _logger = logger.CreateLogger("ErrorHandlerMiddleware"); }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            { await _next(context); }
            catch (Exception ex) { await HandleExceptionAsync(context, env, ex); }
        }

        private Task HandleExceptionAsync(HttpContext context, IWebHostEnvironment env, Exception exception)
        {
            var errorMessage = new StringBuilder();
            context.Response.ContentType = "application/json";

            if (exception is AppException)
            {
                var appException = (exception as AppException);

                context.Response.StatusCode = appException.StatusCode;
                errorMessage.AppendLine(exception.Message);
            }
            else
            {
                context.Response.StatusCode = 500;
                errorMessage.AppendLine($"Request failed with errors! { Environment.NewLine} {(env.IsDevelopment() ? exception.ToString() : exception.Message)}");
            }

            var json = JsonSerializer.Serialize(errorMessage.ToString());
            _logger.LogError(json);

            return context.Response.WriteAsync(json);
        }
    }
}
