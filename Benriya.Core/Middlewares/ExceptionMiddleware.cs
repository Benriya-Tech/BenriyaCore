using Benriya.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Benriya.Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly AppSettings _appSettings;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {                
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var xid = Guid.NewGuid();
                _logger.LogError($"Something went wrong [{xid}]... ");
                _logger.LogError(ex.ToString());
                await HandleExceptionAsync(httpContext,ex, xid, _appSettings.ShowTraceId);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception,Guid xid,bool show_trace = true)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (exception.Message.StartsWith("[") && exception.Message.EndsWith("]"))
            {
                return httpContext.Response.WriteAsync(new ErrorDetails()
                {
                    Status = httpContext.Response.StatusCode,
                    Title = ReasonPhrases.GetReasonPhrase(httpContext.Response.StatusCode),
                    Message = show_trace ? $"[{xid}] {exception.InnerException.Message}" : exception.InnerException.Message,
                    Errors = new Dictionary<string, List<string>>() {{ exception.Message.Split('[', ']')[1], new List<string>() { exception.InnerException.Message }}}
                }.ToString());
            }
            return httpContext.Response.WriteAsync(new ErrorDetails()
            {
                Status = httpContext.Response.StatusCode,
                Title = ReasonPhrases.GetReasonPhrase(httpContext.Response.StatusCode),
                Message = show_trace ? $"[{xid}] {exception.Message}" : exception.Message
            }.ToString());
        }

    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
