using ExpectedObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAToolKit.Engine.HttpTester;
using QAToolKit.Engine.HttpTester.Extensions;
using QAToolKit.xUnit.Fixtures;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.xUnit
{
    public class BicycleApiTests : IClassFixture<BicycleApiSetup>
    {
        private readonly BicycleApiSetup _testSetup;
        private readonly ILogger<BicycleApiTests> _logger;

        public BicycleApiTests(BicycleApiSetup testSetup, ITestOutputHelper testOutputHelper)
        {
            _testSetup = testSetup;

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<BicycleApiTests>();
        }

        [Fact]
        public async Task GetAllBikes_Success()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", _testSetup.ApiVersion } })
                 .WithMethod(HttpMethod.Get)
                 .WithPath("/api/bicycles")
                 .Start();

                var msg = await response.GetResponseBody<List<Bicycle>>();

                _logger.LogInformation(JsonConvert.SerializeObject(msg, Formatting.Indented));

                var expecterResponse = BicycleFixture.GetBicycles().ToExpectedObject();
                expecterResponse.ShouldEqual(msg);

                //Assert.True(client.Duration < 2000);
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async Task GetOneBike_Success()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.HttpRequests.FirstOrDefault(x => x.OperationId == "GetBike"))
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", _testSetup.ApiVersion } })
                 .WithPathReplacementValues( new Dictionary<string, string>() {
                     { "id", "1" }
                 })
                 //.WithMethod(HttpMethod.Get)
                 //.WithPath("/api/bicycles/1")
                 .Start();

                var msg = await response.GetResponseBody<Bicycle>();

                var expecterResponse = BicycleFixture.GetFoil().ToExpectedObject();
                expecterResponse.ShouldEqual(msg);

                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal("Scott", msg.Brand);
            }
        }

        [Fact]
        public async Task GetBikesWithoutHeaders_Success()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", _testSetup.ApiVersion } })
                 .WithMethod(HttpMethod.Post)
                 .WithJsonBody(BicycleFixture.GetCfr())
                 .WithPath("/api/bicycles")
                 .Start();

                var msg = await response.GetResponseBody<Bicycle>();

                var expecterResponse = BicycleFixture.GetCfr().ToExpectedObject();
                expecterResponse.ShouldEqual(msg);

                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal("Giant", msg.Brand);
            }
        }

        [Fact]
        public async Task GetBikesWithoutQueryParams_BadRequest()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithHeaders(new Dictionary<string, string>() { { "Content-Type", "application/json" } })
                 .WithMethod(HttpMethod.Post)
                 .WithJsonBody(BicycleFixture.Get())
                 .WithPath("/api/bicycles")
                 .Start();

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task PostNewBikeWithoutPath_NotFound()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithHeaders(new Dictionary<string, string>() { { "Content-Type", "application/json" } })
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", _testSetup.ApiVersion } })
                 .WithMethod(HttpMethod.Post)
                 .WithJsonBody(BicycleFixture.Get())
                 .Start();

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task PostNewBike_Unauthorized()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithPath("/api/bicycles")
                 .WithHeaders(new Dictionary<string, string>() { { "Content-Type", "application/json" } })
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", "2" } })
                 .WithMethod(HttpMethod.Post)
                 .WithJsonBody(BicycleFixture.Get())
                 .Start();

                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact]
        public async Task PostNewBike_Success()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithPath("/api/bicycles")
                 .WithHeaders(new Dictionary<string, string>() {
                     { "Content-Type", "application/json" } })
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", "2" }, { "apiKey", "12345" } })
                 .WithMethod(HttpMethod.Post)
                 .WithJsonBody(BicycleFixture.Get())
                 .Start();

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }
    }
}
