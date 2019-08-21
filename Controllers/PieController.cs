using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pieshop.Interfaces;
using Pieshop.Models;
using Pieshop.ViewModels;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public class PieController : Controller
    {

        private readonly IPieRepository _pieRepository;
        private readonly IPieReviewRepository _pieReviewRepository;
        private readonly ILogger<PieController> _logger;
        private readonly HtmlEncoder _htmlEncoder;

        public PieController(IPieRepository pieRepository, IPieReviewRepository pieReviewRepository, HtmlEncoder htmlEncoder, ILogger<PieController> logger)
        {
            _pieRepository = pieRepository;
            _pieReviewRepository = pieReviewRepository;
            _htmlEncoder = htmlEncoder; // enocde html input to protect against XSS attacks
            _logger = logger;
        }

        [Route("[controller]/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var pie = await _pieRepository.GetById(id);
            if (pie == null)
            {
                return NotFound();
            }
           
            return View(new PieDetailViewModel { Pie = pie });
        }

        [Route("[controller]/Details/{id}")]
        [HttpPost]
        public async  Task<IActionResult> Details(int id, string review)
        {
            var pie = await _pieRepository.GetById(id);
            if (pie == null)
                return NotFound();

            //html encode the user input - useful for html input fields - ensures malicious script inout cant be re-rendered later 
            try
            {
                string encodedReview = _htmlEncoder.Encode(review);
                await _pieReviewRepository.AddPieReview(new PieReview() { Pie = pie, Review = encodedReview });
            }
            catch(Exception e)
            {
                _logger.LogWarning(e.Message, "Error attempting to html encode user review");
            }

            return View(new PieDetailViewModel() { Pie = pie });
        }


    }
}




//public async Task<IActionResult> Index()
//{
//    var pies = await _pieRepository.GetAllPies();
//    pies = pies.OrderBy(p => p.Name);
//    var vm = new HomeViewModel
//    {
//        Title = "Pies Overview",
//        Pies = pies.ToList()
//    };

//    return View(vm);
//}