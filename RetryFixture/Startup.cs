using Web.Example.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Web.IntegrationTest.RetryFixture
{
    public class Startup
    {
        private static int _callCount;
        private static int _maxFailures;
        public static int MaxFailures
        {
            get => _maxFailures;
            set
            {
                _maxFailures = value;
                _callCount = 0;
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                if (_callCount++ < MaxFailures)
                {
                    context.Response.StatusCode = 503;
                }
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new EchoEmptyResponseModel
                {
                    Url = context.Request.GetDisplayUrl()
                }));
            });
        }
    }
}