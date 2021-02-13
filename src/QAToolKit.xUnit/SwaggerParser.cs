using QAToolKit.Core.Models;
using QAToolKit.Source.Swagger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QAToolKit.xUnit
{
    public static class SwaggerParser
    {
        public static async Task<IEnumerable<HttpRequest>> GetRequests(string swaggerUrl)
        {
            var swaggerSource = new SwaggerUrlSource(
                options =>
                {
                    options.UseSwaggerExampleValues = true;
                });

            return await swaggerSource.Load(new Uri[] {
                   new Uri(swaggerUrl)
            });
        }
    }
}
