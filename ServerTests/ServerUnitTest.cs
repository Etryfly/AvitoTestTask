
using AvitoTestTask;
using AvitoTestTask.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServerTests
{
    public class ServerUnitTest
    {
        private APIController controller;
        public ServerUnitTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var loggerMock = new Mock<ILogger<APIController>>();
            controller = new APIController(loggerMock.Object, serviceProvider.GetService<IMemoryCache>());

        }

        [Fact]
        public async void SetGetTest()
        {

            Pair pair = new Pair { key = "a", value = "b", ttl = 0 };

            controller.Set(pair);

            Assert.Equal(pair, controller.Get(pair.key));
        }

        [Fact]
        public void DelTest()
        {
            Pair pair1 = new Pair { key = "test", value = "b", ttl = 0 };
            Pair pair2 = new Pair { key = "asd", value = "b", ttl = 0 };
            Pair pair3 = new Pair { key = "vbnvbnvbnvbn", value = "b", ttl = 0 };
            controller.Set(pair1);
            controller.Set(pair2);
            controller.Set(pair3);
            List<String> keyList = new List<string> { pair1.key, pair2.key };
            Assert.Equal(keyList.Count, controller.Del(keyList));
            Assert.Equal(controller.Get(pair3.key), pair3);
            Assert.Null(controller.Get(pair1.key).value);
            Assert.Null(controller.Get(pair2.key).value);
        }

        [Fact]
        public void TTLTest()
        {
            Pair pair = new Pair { key = "test", value = "b", ttl = 1 };
            controller.Set(pair);
            System.Threading.Thread.Sleep(2000);
            Assert.Null(controller.Get(pair.key).value);
        }


    }
}
