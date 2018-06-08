using System;
using System.IO;
using System.Linq;
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
                        var objk = GenerateSQLAsync(DQObject.Read(query));
                        var result = JsonConvert.SerializeObject(objk);
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

        /// <summary>
        /// Dummy example - delete!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Task GenerateSQLAsync(DQObject obj)
        {
            if (obj.ElementType == DQElementType.Object)
            {
                obj.SqlQuery += $"SELECT {string.Join(", ", obj.Properties.Where(x => x.ElementType == DQElementType.Value).Select(y => y.Name))} FROM {obj.Name}";
                var ongoing = obj.Properties.Where(x => x.ElementType == DQElementType.Object);
                foreach (DQObject item in ongoing)
                {
                    GenerateSQLAsync(item);
                }
            }

            return Task.FromResult(obj);
        }
    }
}