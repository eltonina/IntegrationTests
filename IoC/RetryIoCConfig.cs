using System;
using System.Net.Http;
using Configuration;
using Configuration.MSTest; 
using Web.Example.Configuration;
using Web.Example.ServiceContracts;
using Web.Example.Services; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configuration.Contracts;
using Web.Client.IoC;
using Web.Client.Extensions;
using Web.Client.HttpHeadersForwarder;

namespace Web.IntegrationTest.IoC
{
    public class RetryIoCConfig
    {
        public static IServiceProvider GetServiceProvider(TestContext context, Func<HttpClient> clientFactory)
        {
            var serviceCollection = new ServiceCollection();

            // Use new extension method to inject both the ServiceGateway configuration and the retry policy
            // configuration
            serviceCollection.TryAddServiceGatewayConfiguration<EchoRetryServiceGatewayClientsConfiguration,
                EchoRetryRetryPolicyConfiguration>();

            serviceCollection.TryAddSingleton<IEchoRetryServiceGateway, EchoRetryServiceGateway>();

            var serviceDescriptor = new ServiceDescriptor(typeof(IHttpClientFactory),
                x => new TestHttpClientFactory(clientFactory), ServiceLifetime.Singleton);
            serviceCollection.Replace(serviceDescriptor);

            serviceDescriptor = new ServiceDescriptor(typeof(ISettingsRepository),
                x => new MSTestSettingsRepository(context), ServiceLifetime.Singleton);
            serviceCollection.Replace(serviceDescriptor);

            serviceCollection.AddScoped<IHttpHeadersForwarderResolver, NullHttpHeadersForwarderResolver>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}