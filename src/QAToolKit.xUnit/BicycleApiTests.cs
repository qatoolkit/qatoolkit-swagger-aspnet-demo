using ExpectedObjects;
using Microsoft.Extensions.Logging;
using QAToolKit.Engine.HttpTester;
using QAToolKit.Engine.HttpTester.Extensions;
using QAToolKit.xUnit.Fixtures;
using System;
using System.Collections.Generic;
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
        public async Task HttpTesterClientSimpleGetAllBikes_Success()
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

                var expecterResponse = BicycleFixture.GetBicycles().ToExpectedObject();
                expecterResponse.ShouldEqual(msg);

                Assert.True(client.Duration < 2000);
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async Task HttpTesterClientSimpleGetOneBike_Success()
        {
            using (var client = new HttpTesterClient())
            {
                var response = await client
                 .CreateHttpRequest(_testSetup.ApiUrl)
                 .WithQueryParams(new Dictionary<string, string>() { { "api-version", _testSetup.ApiVersion } })
                 .WithMethod(HttpMethod.Get)
                 .WithPath("/api/bicycles/1")
                 .Start();

                var msg = await response.GetResponseBody<Bicycle>();

                var expecterResponse = BicycleFixture.GetFoil().ToExpectedObject();
                expecterResponse.ShouldEqual(msg);

                Assert.True(client.Duration < 2000);
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal("Scott", msg.Brand);
            }
        }

        [Fact]
        public async Task HttpTesterClientWithoutHeaders_Success()
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

                Assert.True(client.Duration < 2000);
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal("Giant", msg.Brand);
            }
        }

        [Fact]
        public async Task HttpTesterClientWithoutQueryParams_BadRequest()
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

                Assert.True(client.Duration < 2000);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task HttpTesterClientWithoutPath_NotFound()
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

                Assert.True(client.Duration < 2000);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
