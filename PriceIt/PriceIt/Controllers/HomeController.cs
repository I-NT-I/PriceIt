using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceIt.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PriceIt.Core.Interfaces;
using PriceIt.Core.Services;

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
            //var products = await _webScrapingService.test();

            //ViewData["test"] = await _webScrapingService.GetSaturnProducts();
            //return View(products);
            return View();
        }

        public async Task<ActionResult> Form()
        {

            Request.Query.TryGetValue("field-keywords", out var captcha);
            Request.Query.TryGetValue("amzn", out var amzn);
            Request.Query.TryGetValue("amzn-r", out var amznr);

            var doc = await _webScrapingService.HandelCaptcha(amzn.ToString(), amznr.ToString(), captcha.ToString());

            if (doc != null)
            {
                ViewData["test"] = doc.Text;
            }

            return View();
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
