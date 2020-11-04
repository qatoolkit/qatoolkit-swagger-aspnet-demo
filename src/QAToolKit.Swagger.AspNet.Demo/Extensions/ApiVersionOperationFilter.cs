using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAToolKit.Swagger.AspNet.Demo.Extensions
{
    public class ApiVersionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
           var apiVersion = operation.Parameters.FirstOrDefault(s => s.Name == "api-version");

            if (apiVersion != null)
            {
                using (var outputString = new StringWriter(new StringBuilder("1")))
                {
                    var writer = new OpenApiJsonWriter(outputString);
                    apiVersion.Example = new Microsoft.OpenApi.Any.OpenApiString("1");
                }
            }
        }
    }
}
