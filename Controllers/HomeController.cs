using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pieshop.Interfaces;
using Pieshop.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPieRepository _pieRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IPieRepository pieRepository, ILogger<HomeController> logger)
        {
            _pieRepository = pieRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var pies = await _pieRepository.GetPiesOfTheWeek();
            pies = pies.OrderBy(p => p.Name);
            var vm = new HomeViewModel
            {
                Title = "Pies Of The Week",
                Pies = pies.ToList()
            };

            return View(vm);
        }


        //public async Task<IActionResult> Details(int id)
        //{
        //    var pie = await _pieRepository.GetById(id);
        //    if(pie == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(pie);
        //}

    }
}
