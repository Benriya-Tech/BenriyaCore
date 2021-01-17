using Benriya.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Benriya.Core.Middlewares
{
    public class GCMiddleware
    {
        private readonly RequestDelegate _next;

        public GCMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.OnStarting(() =>
            {
                if (httpContext.Response.StatusCode == 405)
                {
                    httpContext.Response.ContentType = "application/json";
                    string msg = ReasonPhrases.GetReasonPhrase(httpContext.Response.StatusCode);
                    return httpContext.Response.WriteAsync(new ErrorDetails()
                    {
                        Status = httpContext.Response.StatusCode,
                        Title = msg,
                        Message = msg
                    }.ToString());                    
                }
                return Task.CompletedTask;
            });
            await _next(httpContext);
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }
    }

    public static class GCMiddlewareExtensions
    {
        public static IApplicationBuilder UseGCMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GCMiddleware>();
        }
    }
}
