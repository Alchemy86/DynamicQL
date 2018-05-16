using System;
using System.IO;
using System.Threading.Tasks;
using DynamicQL.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DynamicQL.Middleware
{
    public class DynamicQLMiddleware
    {
        private readonly RequestDelegate _next;

        public DynamicQLMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sent = false;
            if (httpContext.Request.Path.StartsWithSegments("/ql"))
            {
                using (var sr = new StreamReader(httpContext.Request.Body))
                {
                    var query = await sr.ReadToEndAsync();
                    if (!String.IsNullOrWhiteSpace(query))
                    {
                        var result = JsonConvert.SerializeObject(DQObject.Read(query));
                        await WriteResult(httpContext, result);
                        sent = true;
                    }
                }
            }
            if (!sent)
            {
                await _next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, string result)
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "text/json";
            await httpContext.Response.WriteAsync(result);
        }
    }
}