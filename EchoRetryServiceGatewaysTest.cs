using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Web.Client.Extensions;
using Web.Example.Configuration;
using Web.Example.ServiceContracts;
using Web.IntegrationTest.IoC;
using Web.IntegrationTest.RetryFixture;

namespace Web.IntegrationTest
{
    [TestClass]
    public class EchoRetryServiceGatewaysTest
    {
        private static IEchoRetryServiceGateway _sut;
        private static EchoRetryRetryPolicyConfiguration _retryPolicyConfiguration;
        private static IServiceProvider _serviceProvider;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var webHostBuilder =
                new WebHostBuilder()
                    .UseStartup<Startup>();

            _testServer = new TestServer(webHostBuilder);

            _serviceProvider = RetryIoCConfig.GetServiceProvider(context, () => _testServer.CreateClient());

            _retryPolicyConfiguration = _serviceProvider.GetService<EchoRetryRetryPolicyConfiguration>();

            _sut = _serviceProvider.GetService<IEchoRetryServiceGateway>();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _testServer.Dispose();
        }

        [TestMethod]
        public async Task GetWithRetry()
        {
            Startup.MaxFailures = 2;

            var results = await _sut.GetWithRetry();
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public async Task GetWithFailedRetry()
        {
            try
            {
                Startup.MaxFailures = 5;

                await _sut.GetWithRetry();
            }
            catch (HttpClientException exception)
            {
                Assert.AreEqual(3, _retryPolicyConfiguration.RetryCount);
                Assert.AreEqual(503, exception.StatusCode);
            }
        }
    }
}
