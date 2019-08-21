using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Pieshop.Interfaces;
using Pieshop.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public class HomeController : ControllerBase
    {

        private readonly IPieRepository _pieRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IPieRepository pieRepository, ILogger<HomeController> logger, IDistributedCache distributedCache) : base(distributedCache)
        {
            _pieRepository = pieRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
   
            _logger.LogWarning("HOME WARNING *****************************************");

            //var pies = await _pieRepository.GetPiesOfTheWeek();
            //pies = pies.OrderBy(p => p.Name);
            //var pies = await LoadPiesOfTheWeek();
            //return ExecuteFunction(() => _accountsService.GetAccountingCompanyById(coyId));

            //load pies of the week - make use of redis distributed cache data
            var pies = await LoadFromDistributedCache("piesOfTheWeek",
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
