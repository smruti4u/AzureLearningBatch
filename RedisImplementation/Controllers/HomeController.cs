using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisImplementation.Models;
using RedisImplementation.Services;

namespace RedisImplementation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICacheService cacheService;

        public HomeController(ILogger<HomeController> logger, ICacheService cacheService)
        {
            _logger = logger;
            this.cacheService = cacheService;
        }

        public IActionResult Index()
        {
            CachedObject obj =  cacheService.GetData<CachedObject>("co").GetAwaiter().GetResult();
            if(obj == null)
            {
                return Content("Cache Has not been set");
            }
            return Content($"Retrieved cache Data : {obj.Name}, {obj.Description}");
           
        }

        public IActionResult SetCache()
        {
            CachedObject cobject = new CachedObject()
            {
                Description = "I am a cached Object",
                Name = "Redis Cache"
            };

            cacheService.SetData<CachedObject>("co", cobject, TimeSpan.FromMinutes(3)).GetAwaiter().GetResult();

            return Content("Cache Has been set");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
