using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceIt.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PriceIt.Core.Interfaces;

namespace PriceIt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebScraping _webScrapingService;

        public HomeController(ILogger<HomeController> logger, IWebScraping webScrapingService)
        {
            _logger = logger;
            _webScrapingService = webScrapingService;
        }

        public async Task<ActionResult> Index()
        {
            var products = await _webScrapingService.GetAmazonProducts();

            return View(products);
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
