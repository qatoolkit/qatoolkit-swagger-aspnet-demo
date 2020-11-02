using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QAToolKit.Swagger.AspNet.Demo.Extensions
{
    public class ConfigureSwaggerApiVersioning : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        readonly string _serviceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerApiVersioning"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerApiVersioning(IApiVersionDescriptionProvider provider, string serviceName)
        {
            _provider = provider;
            _serviceName = serviceName;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"{_serviceName} HTTP API",
                Version = description.ApiVersion.ToString(),
                Description = $"The {_serviceName} Micro service HTTP API."
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
