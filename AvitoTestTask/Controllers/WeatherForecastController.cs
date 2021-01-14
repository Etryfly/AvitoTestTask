using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvitoTestTask.Controllers
{
    [ApiController]
    
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMemoryCache cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            cache = memoryCache;
        }

        [Route("Get")]
        [HttpGet]
        public JsonResult Get(string key)
        {
            return new JsonResult(new
            {
                key = key,
                value = cache.Get(key)
            });
        }

        [Route("Set")]
        [HttpPost]
        public IActionResult Set(String key, String value, int ttl = 0)
        {
            if (ttl == 0)
                cache.Set(key, value);
            else
                cache.Set(key, value, DateTime.Now.AddSeconds(ttl));

            return new EmptyResult();
        }
    }
}
