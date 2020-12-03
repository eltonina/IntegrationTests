using System;
using System.Net.Http;
using Web.Client.Extensions;

namespace Web.IntegrationTest.IoC
{
    public class TestHttpClientFactory : IHttpClientFactory
    {
        private readonly Func<HttpClient> _getClient;

        public TestHttpClientFactory(Func<HttpClient> getClient)
        {
            _getClient = getClient;
        }

        public HttpClient CreateHttpClient(string httpClientName)
        {
            return _getClient.Invoke();
        }
    }
}