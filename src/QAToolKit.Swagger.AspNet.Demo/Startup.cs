using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using QAToolKit.Swagger.AspNet.Demo.Extensions;
using QAToolKit.Swagger.AspNet.Demo.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace QAToolKit.Swagger.AspNet.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(
               o =>
               {
                   o.AssumeDefaultVersionWhenUnspecified = false;
                   o.DefaultApiVersion = new ApiVersion(2, 0);
                   o.ReportApiVersions = true;
                   o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader()
                       , new HeaderApiVersionReader(new string[] { $"X-version" }));
               });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.DescribeAllEnumsAsStrings();
            });

            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>>(provider =>
                new ConfigureSwaggerApiVersioning(
                    provider.GetService<IApiVersionDescriptionProvider>(),
                    serviceName: "QAToolKit.Swagger.AspNet.Demo")
            );

            services.AddSingleton<BikeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(o => o.GroupName))
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
