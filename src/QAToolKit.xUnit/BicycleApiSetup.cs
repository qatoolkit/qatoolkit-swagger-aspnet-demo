using QAToolKit.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QAToolKit.xUnit
{
    public class BicycleApiSetup
    {
        public readonly string ApiVersion = "1.0";
        public readonly Uri ApiUrl = new Uri("https://qatoolkitapi.azurewebsites.net");
        
        /*public readonly IEnumerable<HttpRequest> HttpRequests;

        public BicycleApiSetup()
        {
            HttpRequests = SwaggerParser.GetRequests("https://qatoolkitapi.azurewebsites.net/swagger/v2/swagger.json").ConfigureAwait(false).GetAwaiter().GetResult();
        }*/
    }
}
