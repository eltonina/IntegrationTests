using System;
using Configuration.MSTest;
using Web.Example.Configuration;
using Web.Example.ServiceContracts;
using Web.Example.Services; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configuration.Contracts;
using Web.Client.IoC;
using Web.Client.HttpHeadersForwarder;
using Web.Example.IoCDefaults;

namespace Web.IntegrationTest.Ioc
{
    public static class IoCConfig
    {

        // The Service provider needs a TestContext in order to access to the run settings
        public static IServiceProvider GetServiceProvider(this TestContext context)
        {
            var serviceCollection = new ServiceCollection();

            // Register the configuration, when a configuration register is done,
            // all the dependencies for the service gateways are registered automatically
            serviceCollection.TryAddServiceGatewayConfiguration<EchoServiceGatewayClientsConfiguration>();

            // Register the ServiceGateways, normally you must register as Singleton but in some case you must use Scoped.
            serviceCollection.TryAddSingleton<IEchoServiceGateway, EchoServiceGateway>();
            serviceCollection.TryAddSingleton<IEchoCRUDServiceGateway, EchoCRUDServiceGateway>();

            // but if you want, instead 26, 29 and 30 lines you can use a simple method that register all service required
            // see the EchosServiceCollectionServiceGatewayExtensions class to know how implement it
             serviceCollection.RegisterEchoServiceGateways();

            // In case of test usage you must replace de default implementation of ISettingsRepository
            var serviceDescriptor = new ServiceDescriptor(typeof(ISettingsRepository), x => new MSTestSettingsRepository(context), ServiceLifetime.Singleton);
            serviceCollection.Replace(serviceDescriptor);

            // If you don't want support the micro-services headers transference register NullHttpHeadersForwarderResolver as IHttpHeadersForwarderResolver
            serviceCollection.AddScoped<IHttpHeadersForwarderResolver, NullHttpHeadersForwarderResolver>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }

}