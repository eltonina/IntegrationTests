using System;
using System.Threading.Tasks;
using Web.Example.Models;
using Web.Example.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.IntegrationTest.Ioc;

namespace Web.IntegrationTest
{
    [TestClass]
    public class EchoServiceGatewaysTest
    {
        private static IEchoServiceGateway _sut1;
        private static IEchoCRUDServiceGateway _sut2;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var serviceProvider = context.GetServiceProvider();

            _sut1 = serviceProvider.GetService<IEchoServiceGateway>();
            _sut2 = serviceProvider.GetService<IEchoCRUDServiceGateway>();
        }

        [TestMethod]
        public async Task GetWithoutQueryString()
        {
            var results = await _sut1.GetWithoutQueryString();
            Assert.IsNotNull(results);
            Assert.AreEqual("https://postman-echo.com/get?xCliTrack=HTTPCLIENTV2_EchoServiceGatewayIntegrationTest", results.Url);
        }

        [TestMethod]
        public async Task Post()
        {
            var echoRequestModel = new EchoRequestModel
            {
                Name = "Perry",
                Surname = "the Platypus",
                Age = 11,
                Enabled = true,
                Birthday = new DateTime(2020, 12, 01)
            };

            var results = await _sut2.Post(echoRequestModel);
            Assert.IsNotNull(results);
            Assert.AreEqual(echoRequestModel.Name, results.Data.Name);
            Assert.AreEqual(echoRequestModel.Surname, results.Data.Surname);
            Assert.AreEqual(echoRequestModel.Age, results.Data.Age);
            Assert.AreEqual(echoRequestModel.Birthday, results.Data.Birthday);
            Assert.AreEqual(echoRequestModel.Enabled, results.Data.Enabled);
        }

        [TestMethod]
        public async Task Put()
        {
            var echoRequestModel = new EchoRequestModel
            {
                Name = "Perry",
                Surname = "the Platypus",
                Age = 11,
                Enabled = true,
                Birthday = new DateTime(2222, 01, 01)
            };

            var results = await _sut2.Put(echoRequestModel);
            Assert.IsNotNull(results);
            Assert.AreEqual(echoRequestModel.Name, results.Data.Name);
            Assert.AreEqual(echoRequestModel.Surname, results.Data.Surname);
            Assert.AreEqual(echoRequestModel.Age, results.Data.Age);
            Assert.AreEqual(echoRequestModel.Birthday, results.Data.Birthday);
            Assert.AreEqual(echoRequestModel.Enabled, results.Data.Enabled);
        }

        [TestMethod]
        public async Task Get()
        {
            var echoRequestModel = new EchoRequestModel
            {
                Name = "Perry",
                Surname = "the Platypus",
                Age = 11,
                Enabled = true,
                Birthday = new DateTime(2021, 01, 01)
            };

            var results = await _sut2.Get(echoRequestModel);
            Assert.IsNotNull(results);
            Assert.AreEqual(echoRequestModel.Name, results.Args.Name);
            Assert.AreEqual(echoRequestModel.Surname, results.Args.Surname);
            Assert.AreEqual(echoRequestModel.Age, results.Args.Age);
            Assert.AreEqual(echoRequestModel.Birthday, results.Args.Birthday);
            Assert.AreEqual(echoRequestModel.Enabled, results.Args.Enabled);
        }

        [TestMethod]
        public async Task Delete()
        {
            var echoRequestModel = new EchoRequestModel
            {
                Name = "Perry",
                Surname = "the Platypus",
                Age = 11,
                Enabled = true,
                Birthday = new DateTime(2021, 01, 01)
            };

            await _sut2.Delete(echoRequestModel);
        }
    }
}
