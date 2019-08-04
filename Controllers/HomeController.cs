using Microsoft.AspNetCore.Mvc;
using Pieshop.Interfaces;
using Pieshop.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public async Task<IActionResult> Index()
        {
            var pies = await _pieRepository.GetAllPies();
            pies = pies.OrderBy(p => p.Name);
            var vm = new HomeViewModel
            {
                Title = "Pies Overview",
                Pies = pies.ToList()
            };

            return View(vm);
        }


        public async Task<IActionResult> Details(int id)
        {
            var pie = await _pieRepository.GetById(id);
            if(pie == null)
            {
                return NotFound();
            }
            return View(pie);
        }

    }
}
