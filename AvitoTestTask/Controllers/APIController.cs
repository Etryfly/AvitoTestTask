
using AvitoTestTask;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AvitoTestTask.Controllers
{
    [ApiController]

    public class APIController : ControllerBase
    {


        private readonly ILogger<APIController> _logger;
        private readonly IMemoryCache cache;


        public APIController(ILogger<APIController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            cache = memoryCache;

        }

        [Route("Get")]
        [HttpGet]
        public Pair Get(string key)
        {
            _logger.LogInformation(key);
            return new Pair{ key=key, value=(string)cache.Get(key),ttl= 0 } ;
           
        }

        [Route("Set")]
        [HttpPost]
        public IActionResult Set([FromBody] Pair data)
        {

            if (data.ttl == 0)
                cache.Set(data.key, data.value);
            else
                cache.Set(data.key, data.value, DateTime.Now.AddSeconds(data.ttl));


            return new EmptyResult();
        }

        [Route("Keys")]
        [HttpGet]
        public JsonResult Keys(string pattern)
        {
            _logger.LogInformation(pattern);
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(cache) as ICollection;
            var items = new List<string>();
            if (collection != null)
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    
                    if (pattern == null || new Regex(Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".")).IsMatch(val.ToString()))
                    {
                        items.Add(val.ToString());
                    }
                }
            return new JsonResult(items);
        }

        [Route("Del")]
        [HttpPost]
        public int Del(List<string> keyList)
        {
            int count = 0;
            foreach (string key in keyList)
            {
                if (cache.Get(key) != null)
                {
                    count++;
                    cache.Remove(key);
                }
            }

            return count;
        }

    }
}
