using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pieshop.Interfaces;
using Pieshop.Models;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackRepository feedbackRepository, ILogger<FeedbackController> logger)
        {
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(Feedback feedback)
        {

            if (ModelState.IsValid)
            { 
                await _feedbackRepository.AddFeedback(feedback);
                return RedirectToAction("FeedbackComplete");
            }

            return View(feedback);
      
        }

        [Authorize]
        public IActionResult FeedbackComplete()
        {
            return View();
        }



    }
}