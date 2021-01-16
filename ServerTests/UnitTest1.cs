
using Xunit;
using AvitoTestTaskClient;
using System;
using System.Threading.Tasks;
using AvitoTestTask.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using AvitoTestTask;
using Microsoft.AspNetCore.Mvc;

namespace ServerTests
{
    public class UnitTest1
    {
        [Fact]
        public async void SetGetTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var loggerMock = new Mock<ILogger<APIController>>();
            APIController controller = new APIController(loggerMock.Object, serviceProvider.GetService<IMemoryCache>());
            Pair pair = new Pair { key="a", value="b", ttl=0 };
      
            controller.Set(pair);


            Assert.Equal(pair, controller.Get(pair.key));
        }

       
    }
}
