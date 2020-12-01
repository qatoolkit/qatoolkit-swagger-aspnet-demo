using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace QAToolKit.Swagger.AspNet.Demo.Extensions
{
    public sealed class ApiKeyAuthorizationFilterAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthorizationFilterAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKeys = new List<string>
            {
                context.HttpContext.Request?.Query["ApiKey"].ToString()
            };

            //validate Api Key
            var foundValidApiKey = false;
            foreach (var apiKey in apiKeys)
            {
                if (apiKey == _configuration["ApiKey"])
                {
                    foundValidApiKey = true;
                    break;
                }
            }

            if (!foundValidApiKey)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
                return;
            }
        }
    }
}
