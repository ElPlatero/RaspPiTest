using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RaspPiTest.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _nextRequestDelegate;
        private readonly ILogger _logger;

        public ErrorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _nextRequestDelegate = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ErrorMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _nextRequestDelegate(context);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex.Message);
                await HandleApiExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                var exception = ApiException.Create(ex);
                await HandleApiExceptionAsync(context, exception);
            }
        }

        private async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                throw exception;
            }

            context.Response.Clear();
            context.Response.StatusCode = exception.StatusCode;
            context.Response.ContentType = exception.ContentType;
            await context.Response.WriteAsync(exception.Message);
        }
    }
}