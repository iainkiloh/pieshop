using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pieshop.Interfaces;
using Pieshop.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPieRepository _pieRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCacheService _distributedCacheService;

        public HomeController(IPieRepository pieRepository, ILogger<HomeController> logger, IDistributedCacheService distributedCacheService) 
        {
            _pieRepository = pieRepository;
            _logger = logger;
            _distributedCacheService = distributedCacheService;
        }

        [IgnoreAntiforgeryToken] //id response caching => required to ignore the anti-forgery token - ok for views which you want to cache, and which have no post form
        [ResponseCache(Duration = 20, VaryByHeader = "User-Agent", NoStore = false, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            //test logging
            _logger.LogWarning("HOME WARNING *****************************************");

            //load pies of the week - make use of redis distributed cache data
            var pies = await _distributedCacheService.LoadFromDistributedCache("piesOfTheWeek",
                TimeSpan.FromMinutes(20),
                () => _pieRepository.GetPiesOfTheWeek()); 

            var vm = new HomeViewModel
            {
                Title = "Pies Of The Week",
                Pies = pies.ToList()
            };

            return View(vm);
        }
 
    }
}
