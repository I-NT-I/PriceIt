using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceIt.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PriceIt.Core.Interfaces;
using PriceIt.Data.Interfaces;
using PriceIt.Data.Models;

namespace PriceIt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebScraping _webScrapingService;
        private readonly IProductsRepository _productsRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IWebScraping webScrapingService,IProductsRepository productsRepository,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _webScrapingService = webScrapingService;
            _productsRepository = productsRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ActionResult> Index()
        {
            //ViewData["test"] = await _webScrapingService.GetAmazonProducts();

            RecurringJob.AddOrUpdate(() => _webScrapingService.ScrapAllWebSites(), "* 05 * * *", TimeZoneInfo.Local);

            return View();
        }

        public IActionResult Login()
        {
            var viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.UserName);

                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("UserName", "Username or password do not match!");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Username or password do not match!");
                }
            }

            return View(viewModel);
        }

        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Password != viewModel.PasswordCheck)
                {
                    ModelState.AddModelError("PasswordCheck","Passwords do not match!");
                    return View(viewModel);
                }

                try
                {
                    var email = new MailAddress(viewModel.Email);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Email","Provide a correct Email address");

                    return View(viewModel);
                }

                var user = new AppUser
                {
                    UserName = viewModel.UserName,
                    Email = viewModel.Email,
                    UserLists = new List<UserList>()
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }

                if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    ModelState.AddModelError("UserName","Username already in use");
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Search()
        {
            var searchViewModel = new SearchResultViewModel
            {
                Query = "",
                Products = _productsRepository.Search("","",new List<string>())
            };

            return View(searchViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Search(string query, string website, List<string> categories)
        {
            var searchViewModel = new SearchResultViewModel {Query = query,Products = _productsRepository.Search(query,website, categories) };

            return View(searchViewModel);
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
