using QAToolKit.Auth.AzureB2C;
using System;
using System.Threading.Tasks;

namespace QAToolKit.Demo.ServiceTests
{
    public static class Auth
    {
        public static async Task<string> GetAuthenticator()
        {

           var azureB2CAuthenticator = new AzureB2CAuthenticator(options =>
            {
                options.AddClientCredentialFlowParameters(
                    new Uri("https://<tenant-name>.b2clogin.com/<tenant-name>.onmicrosoft.com/<policy-name>/oauth2/v2.0/token"),
                    "753985f4-c75f-40a1-85d6-1a0e7ce68851",
                    "12345");
            });

            return await azureB2CAuthenticator.GetAccessToken();
        }
    }
}
