using System;
using System.IO;
using System.Threading.Tasks;
using DynamicQL.Core;
using DynamicQL.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DynamicQL.Middleware
{
    public class DynamicQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDynamicQLConverter _qlConverter;

        public DynamicQLMiddleware(RequestDelegate next, IDynamicQLConverter qlConverter)
        {
            _next = next;
            _qlConverter = qlConverter;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sent = false;
            if (httpContext.Request.Path.StartsWithSegments("/ql"))
            {
                using (var sr = new StreamReader(httpContext.Request.Body))
                {
                    var query = await sr.ReadToEndAsync();
                    _qlConverter.Convert(query);
                    if (!String.IsNullOrWhiteSpace(query))
                    {
                        var result = await new QueryExecuter().ExecuteAsync().ConfigureAwait(false);
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
            httpContext.Response.ContentType = "text";
            await httpContext.Response.WriteAsync(result);
        }
    }
}