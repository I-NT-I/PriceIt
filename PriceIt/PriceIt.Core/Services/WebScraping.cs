using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using PriceIt.Core.Interfaces;
using PriceIt.Core.Models;
using PriceIt.Data.Interfaces;
using PriceIt.Data.Models;

namespace PriceIt.Core.Services
{
    public class WebScraping : IWebScraping
    {
        private const string BaseUrlAmazon = "https://www.amazon.de";
        private const string BaseUrlMediaMarkt = "https://www.mediamarkt.de";
        private const string BaseUrlSaturn = "https://www.saturn.de";

        private readonly int _pagesToScrap = 2;

        private readonly IHttpCallManager _callManager;
        private readonly ICSVStore _csvStore;
        private readonly IProductsRepository _productsRepository;

        public WebScraping(IHttpCallManager callManager, ICSVStore csvStore, IProductsRepository productsRepository)
        {
            _callManager = callManager;
            _csvStore = csvStore;
            _productsRepository = productsRepository;
        }

        public async Task ScrapAllWebSites()
        {
            await GetAmazonProducts();
            await GetMediaMarktProducts();
            await GetSaturnProducts();
        }

        public async Task GetAmazonProducts()
        {
            var playwright = await Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome" ,Headless = false});

            var context = await browser.NewContextAsync();

            //scraping amazon cpus
            var cpus = await GetAmazonProductsFromCategory(context, Category.CPU,
                "https://www.amazon.de/b/?ie=UTF8&node=430177031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=E3X5BXZBEVWQ029HVY7H&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430177031_6");

            if (!cpus.Any())
                Console.WriteLine("Warning Amazon - CPUS");

            try
            {
                foreach (var cpu in cpus)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(cpu);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Amazon - CPUS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Amazon CPUS");
            }

            //scraping amazon rams
            var rams = await GetAmazonProductsFromCategory(context, Category.RAM,
                "https://www.amazon.de/b/?ie=UTF8&node=430178031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=EGZMRJ05ENZ3PS2T3PCJ&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430178031_5");

            if (!rams.Any())
                Console.WriteLine("Warning Amazon - RAMS");

            try
            {
                foreach (var ram in rams)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(ram);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Amazon - RAMS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Amazon RAMS");
            }

            //scraping amazon power supplies
            var powerSupplies = await GetAmazonStorageProducts(context, Category.PowerSupply,
                "https://www.amazon.de/b/?ie=UTF8&node=430176031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=V0TVRYQD99TQJJSA6Q5X&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430176031_4");

            if (!powerSupplies.Any())
                Console.WriteLine("Warning Amazon - POWERSUPPLIES");

            try
            {
                foreach (var powerSupply in powerSupplies)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(powerSupply);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Amazon - POWERSUPPLIES");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Amazon POWERSUPPLIES");
            }

            //scraping amazon GPUs
            var gpus = await GetAmazonProductsFromCategory(context, Category.GraphicCard,
                "https://www.amazon.de/b/?ie=UTF8&node=430161031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=54VKGX4QWDDS37BA6J14&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430161031_3");

            if (!gpus.Any())
                Console.WriteLine("Warning Amazon - GPUS");

            try
            {
                foreach (var gpu in gpus)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(gpu);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Amazon - GPUS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Amazon GPUS");
            }

            //scraping amazon Motherboards
            var motherBoards = await GetAmazonProductsFromCategory(context, Category.MotherBoard,
                "https://www.amazon.de/b/?ie=UTF8&node=430172031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=J340MH7BRRJ5FEABNEYG&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430172031_2");

            if (!motherBoards.Any())
                Console.WriteLine("Warning Amazon - MotherBoards");

            try
            {
                foreach (var motherBoard in motherBoards)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(motherBoard);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Amazon - MotherBoards");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Amazon MotherBoards");
            }

            //scraping amazon Storage
            var storage = await GetAmazonProductsFromCategory(context, Category.Storge,
                "https://www.amazon.de/b/?ie=UTF8&node=430168031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=9PQ81844TAW65ZWMWF7K&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430168031_1");

            if (!storage.Any())
                Console.WriteLine("Warning Amazon - Storage");

            try
            {
                foreach (var item in storage)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(item);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Amazon - Storage");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Amazon Storage");
            }
        }

        private async Task<List<Product>> GetAmazonProductsFromCategory(IBrowserContext context, Category category, string categoryUrl)
        {
            var products = new List<Product>();

            var page = await context.NewPageAsync();
            await page.GotoAsync(categoryUrl);

            var nextPageBlock = await page.QuerySelectorAsync("//li[@class='a-last']");
            var pageNumber = 0;

            while (nextPageBlock != null && pageNumber < _pagesToScrap)
            {
                var elements = await page.QuerySelectorAllAsync(
                    "//div[contains(concat(' ',normalize-space(@class),' '),'s-result-item s-asin')]");

                foreach (var element in elements)
                {
                    var product = new Product();

                    //Getting ASIN
                    product.ProductIdentifier = await element.GetAttributeAsync("data-asin");

                    //Getting the name of the product
                    var nameBlock = await element.QuerySelectorAsync("//span[@class='a-size-medium a-color-base a-text-normal']");
                    if (nameBlock == null) continue;

                    var name = await nameBlock.TextContentAsync();
                    if (string.IsNullOrEmpty(name)) continue;

                    product.Name = name;

                    //Getting the image of the product if found
                    var image = "";
                    var imageBlock = await element.QuerySelectorAsync("//div[@class='a-section aok-relative s-image-fixed-height']");
                    if (imageBlock != null)
                    {
                        var img = await imageBlock.QuerySelectorAsync("//img[@class='s-image']");
                        if (img != null)
                        {
                            image = await img.GetAttributeAsync("src");
                        }
                    }

                    product.ProductImageUrl = image;

                    //Getting the Url to the product details page
                    var urlBLock = await element.QuerySelectorAsync("//a[@class='a-link-normal s-no-outline']");
                    if (urlBLock == null) continue;

                    var url = await urlBLock.GetAttributeAsync("href");

                    if (string.IsNullOrEmpty(url)) continue;

                    product.ProductUrl = url;

                    //Getting the price of the product
                    var priceBlock = await element.QuerySelectorAsync("//span[@class='a-price-whole']");
                    if (priceBlock == null) continue;

                    var priceValue = await priceBlock.TextContentAsync();

                    if (string.IsNullOrEmpty(priceValue)) continue;

                    if (!float.TryParse(priceValue, out var result)) continue;

                    product.Price = result;

                    product.Category = category;

                    product.Website = Website.Amazon;

                    var now = DateTime.Now;
                    product.LastUpdate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                    products.Add(product);
                }

                nextPageBlock = await page.QuerySelectorAsync("//li[@class='a-last']");
                if (nextPageBlock != null)
                    await nextPageBlock.ClickAsync();
                pageNumber++;

                Thread.Sleep(2500);
            }

            await page.CloseAsync();

            return products;
        }

        private async Task<List<Product>> GetAmazonStorageProducts(IBrowserContext context, Category category, string categoryUrl)
        {
            var products = new List<Product>();

            var page = await context.NewPageAsync();
            await page.GotoAsync(categoryUrl);

            var nextPageBlock = await page.QuerySelectorAsync("//li[@class='a-last']");
            var pageNumber = 0;

            while (nextPageBlock != null && pageNumber < _pagesToScrap)
            {
                var elements = await page.QuerySelectorAllAsync(
                    "//div[contains(concat(' ',normalize-space(@class),' '),'s-result-item s-asin')]");

                foreach (var element in elements)
                {
                    var product = new Product();

                    //Getting ASIN
                    product.ProductIdentifier = await element.GetAttributeAsync("data-asin");

                    //Getting the name of the product
                    var nameBlock = await element.QuerySelectorAsync("//span[@class='a-size-base-plus a-color-base a-text-normal']");
                    if (nameBlock == null) continue;

                    var name = await nameBlock.TextContentAsync();
                    if (string.IsNullOrEmpty(name)) continue;

                    product.Name = name;

                    //Getting the image of the product if found
                    var image = "";
                    var imageBlock = await element.QuerySelectorAsync("//div[@class='a-section aok-relative s-image-square-aspect']");
                    if (imageBlock != null)
                    {
                        var img = await imageBlock.QuerySelectorAsync("//img[@class='s-image']");
                        if (img != null)
                        {
                            image = await img.GetAttributeAsync("src");
                        }
                    }

                    product.ProductImageUrl = image;

                    //Getting the Url to the product details page
                    var urlBLock = await element.QuerySelectorAsync("//a[@class='a-link-normal s-no-outline']");
                    if (urlBLock == null) continue;

                    var url = await urlBLock.GetAttributeAsync("href");

                    if (string.IsNullOrEmpty(url)) continue;

                    product.ProductUrl = url;

                    //Getting the price of the product
                    var priceBlock = await element.QuerySelectorAsync("//span[@class='a-price-whole']");
                    if (priceBlock == null) continue;

                    var priceValue = await priceBlock.TextContentAsync();

                    if (string.IsNullOrEmpty(priceValue)) continue;

                    if (!float.TryParse(priceValue, out var result)) continue;

                    product.Price = result;

                    product.Category = category;

                    product.Website = Website.Amazon;

                    var now = DateTime.Now;
                    product.LastUpdate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                    products.Add(product);
                }

                nextPageBlock = await page.QuerySelectorAsync("//li[@class='a-last']");
                if (nextPageBlock != null)
                    await nextPageBlock.ClickAsync();
                pageNumber++;

                Thread.Sleep(2500);
            }

            await page.CloseAsync();

            return products;
        }

        public async Task GetMediaMarktProducts()
        {
            var products = new List<Product>();

            using var playwright = await Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = false });

            var context = await browser.NewContextAsync();

            //scraping MediaMarkt cpus
            var cpus = await GetMediaMarktProductsFromCategory(context, Category.CPU,
                "https://www.mediamarkt.de/de/category/_prozessoren-cpu-692537.html?page=");

            if (!cpus.Any())
                Console.WriteLine("Warning MediaMarkt - CPUS");

            try
            {
                foreach (var cpu in cpus)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(cpu);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning MediaMarkt - CPUS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "MediaMarkt CPUS");
            }

            //scraping MediaMarkt rams
            var rams = await GetMediaMarktProductsFromCategory(context, Category.RAM,
                "https://www.mediamarkt.de/de/category/_arbeitsspeicher-ram-462907.html?page=");

            if (!rams.Any())
                Console.WriteLine("Warning MediaMarkt - RAMS");

            try
            {
                foreach (var ram in rams)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(ram);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning MediaMarkt - RAMS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "MediaMarkt RAMS");
            }

            //scraping MediaMarkt power supplies
            var powerSupplies = await GetMediaMarktProductsFromCategory(context, Category.PowerSupply,
                "https://www.mediamarkt.de/de/search.html?query=pc%20power%20supply&page=");

            if (!powerSupplies.Any())
                Console.WriteLine("Warning MediaMarkt - POWERSUPPLIES");

            try
            {
                foreach (var powerSupply in powerSupplies)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(powerSupply);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning MediaMarkt - POWERSUPPLIES");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "MediaMarkt POWERSUPPLIES");
            }

            //scraping MediaMarkt GPUs
            var gpus = await GetMediaMarktProductsFromCategory(context, Category.GraphicCard,
                "https://www.mediamarkt.de/de/category/_grafikkarten-640610.html?page=");

            if (!gpus.Any())
                Console.WriteLine("Warning MediaMarkt - GPUS");

            try
            {
                foreach (var gpu in gpus)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(gpu);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning MediaMarkt - GPUS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "MediaMarkt GPUS");
            }

            //scraping MediaMarkt Motherboards
            var motherBoards = await GetMediaMarktProductsFromCategory(context, Category.MotherBoard,
                "https://www.mediamarkt.de/de/category/_mainboards-691008.html?page=");

            if (!motherBoards.Any())
                Console.WriteLine("Warning MediaMarkt - MotherBoards");

            try
            {
                foreach (var motherBoard in motherBoards)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(motherBoard);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning MediaMarkt - MotherBoards");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "MediaMarkt MotherBoards");
            }

            //scraping MediaMarkt Storage
            var storage = await GetMediaMarktProductsFromCategory(context, Category.Storge,
                "https://www.mediamarkt.de/de/category/_festplatten-686016.html?page=");

            if (!storage.Any())
                Console.WriteLine("Warning MediaMarkt - Storage");

            try
            {
                foreach (var item in storage)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(item);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning MediaMarkt - Storage");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "MediaMarkt Storage");
            }
        }

        private async Task<List<Product>> GetMediaMarktProductsFromCategory(IBrowserContext context, Category category, string categoryUrl)
        {
            var products = new List<Product>();

            var page = await context.NewPageAsync();

            var pageNumber = 1;

            await page.GotoAsync(categoryUrl + pageNumber);

            var loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

            while (loadMoreBlock != null && pageNumber <= _pagesToScrap)
            {
                var elements = await page.QuerySelectorAllAsync("//div[@class='ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw']");

                foreach (var element in elements)
                {
                    try
                    {

                        var product = new Product();

                        await element.ScrollIntoViewIfNeededAsync();

                        //Getting the name of the product
                        var nameBlock = await element.QuerySelectorAsync("//p[@class='Typostyled__StyledInfoTypo-sc-1jga2g7-0 fuXjPV']");
                        if (nameBlock == null) continue;

                        var spanNameBlock = await element.QuerySelectorAsync("//span[@class='Typostyled__StyledInfoTypo-sc-1jga2g7-0 kPKTpQ']");
                        if (spanNameBlock == null) continue;

                        var name = await spanNameBlock.TextContentAsync() + await nameBlock.TextContentAsync();
                        if (string.IsNullOrEmpty(name)) continue;

                        product.Name = name;

                        //Getting the image of the product if found
                        var image = "";
                        var imageBlock = await element.QuerySelectorAsync("//div[@class='Picturestyled__StyledPicture-sc-1s3zfhk-0 hwMBxB']");
                        if (imageBlock != null)
                        {
                            var img = await imageBlock.QuerySelectorAsync("//img");
                            if (img != null)
                            {
                                image = await img.GetAttributeAsync("src");
                            }
                        }

                        product.ProductImageUrl = image;

                        //Getting the Url to the product details page
                        var urlBLock = await element.QuerySelectorAsync("//a[@class='Linkstyled__StyledLinkRouter-sc-1drhx1h-2 iDDAGF ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV']");
                        if (urlBLock == null) continue;

                        var url = await urlBLock.GetAttributeAsync("href");

                        if (string.IsNullOrEmpty(url)) continue;

                        product.ProductUrl = url;

                        //Getting the price of the product
                        var priceBlock = await element.QuerySelectorAsync("//span[@aria-hidden='true']");
                        if (priceBlock == null) continue;

                        var priceSpans = await priceBlock.QuerySelectorAllAsync("//*");

                        if (!priceSpans.Any()) continue;

                        var priceEuro = await priceSpans[0].TextContentAsync();
                        if (priceEuro == null) continue;

                        priceEuro = priceEuro.Replace(".", "");
                        if (!float.TryParse(priceEuro, out var euroResult)) continue;

                        switch (priceSpans.Count)
                        {
                            case 2:
                                {
                                    var priceCent = await priceSpans[1].TextContentAsync();
                                    if (priceCent == null) continue;

                                    if (!float.TryParse(priceCent, out var centResult)) continue;

                                    product.Price = euroResult + (centResult / 100);
                                    break;
                                }
                            case 1:
                                product.Price = euroResult;
                                break;
                            default: continue;
                        }

                        product.Category = category;

                        product.Website = Website.MediaMarkt;

                        var now = DateTime.Now;
                        product.LastUpdate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                        products.Add(product);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                try
                {
                    pageNumber++;

                    var nextPage = await page.GotoAsync(categoryUrl + pageNumber);

                    if (nextPage == null || !nextPage.Ok) continue;

                    loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

                    Thread.Sleep(2500);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            await page.CloseAsync();

            return products;
        }

        public async Task GetSaturnProducts()
        {
            using var playwright = await Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = false });

            var context = await browser.NewContextAsync();

            //scraping Saturn cpus
            var cpus = await GetSaturnProducts(context, Category.CPU,
                "https://www.saturn.de/de/category/_prozessoren-cpu-693063.html?page=");

            if (!cpus.Any())
                Console.WriteLine("Warning Saturn - CPUS");

            try
            {
                foreach (var cpu in cpus)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(cpu);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Saturn - CPUS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Saturn CPUS");
            }

            //scraping Saturn rams
            var rams = await GetSaturnProducts(context, Category.RAM,
                "https://www.saturn.de/de/category/_arbeitsspeicher-286900.html?page=");

            if (!rams.Any())
                Console.WriteLine("Warning Saturn - RAMS");

            try
            {
                foreach (var ram in rams)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(ram);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Saturn - RAMS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Saturn RAMS");
            }

            //scraping Saturn power supplies
            var powerSupplies = await GetSaturnProducts(context, Category.PowerSupply,
                "https://www.saturn.de/de/search.html?query=pc%20power%20supply&page=");

            if (!powerSupplies.Any())
                Console.WriteLine("Warning Saturn - POWERSUPPLIES");

            try
            {
                foreach (var powerSupply in powerSupplies)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(powerSupply);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Saturn - POWERSUPPLIES");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Saturn POWERSUPPLIES");
            }

            //scraping Saturn GPUs
            var gpus = await GetSaturnProducts(context, Category.GraphicCard,
                "https://www.saturn.de/de/category/_grafikkarten-286896.html?page=");

            if (!gpus.Any())
                Console.WriteLine("Warning Saturn - GPUS");

            try
            {
                foreach (var gpu in gpus)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(gpu);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Saturn - GPUS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Saturn GPUS");
            }

            //scraping Saturn Motherboards
            var motherBoards = await GetSaturnProducts(context, Category.MotherBoard,
                "https://www.saturn.de/de/category/_mainboards-527446.html?page=");

            if (!motherBoards.Any())
                Console.WriteLine("Warning Saturn - MotherBoards");

            try
            {
                foreach (var motherBoard in motherBoards)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(motherBoard);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Saturn - MotherBoards");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Saturn MotherBoards");
            }

            //scraping Saturn Storage
            var storage = await GetSaturnProducts(context, Category.Storge,
                "https://www.saturn.de/de/category/_interne-festplatten-286924.html?page=");

            if (!storage.Any())
                Console.WriteLine("Warning Saturn - Storage");

            try
            {
                foreach (var item in storage)
                {
                    try
                    {
                        await _productsRepository.AddProductAsync(item);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (!_productsRepository.Save())
                    Console.WriteLine("Warning Saturn - Storage");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Saturn Storage");
            }
        }

        private async Task<List<Product>> GetSaturnProducts(IBrowserContext context,Category category,string categoryUrl)
        {
            var products = new List<Product>();

            var page = await context.NewPageAsync();

            var pageNumber = 1;

            await page.GotoAsync(categoryUrl + pageNumber);

            var loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

            while (loadMoreBlock != null && pageNumber <= _pagesToScrap)
            {
                var elements =
                    await page.QuerySelectorAllAsync("//div[@class='ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw']");

                foreach (var element in elements)
                {
                    try
                    {
                        var product = new Product();

                        await element.ScrollIntoViewIfNeededAsync();

                        //Getting the name of the product
                        var nameBlock = await element.QuerySelectorAsync("//p[@class='Typostyled__StyledInfoTypo-sc-1jga2g7-0 fuXjPV']");
                        if (nameBlock == null) continue;

                        var spanNameBlock = await element.QuerySelectorAsync("//span[@class='Typostyled__StyledInfoTypo-sc-1jga2g7-0 kDlgid']");
                        if (spanNameBlock == null) continue;

                        var name = await spanNameBlock.TextContentAsync() + await nameBlock.TextContentAsync();
                        if (string.IsNullOrEmpty(name)) continue;

                        product.Name = name;

                        //Getting the image of the product if found
                        var image = "";
                        var imageBlock = await element.QuerySelectorAsync("//div[@class='Picturestyled__StyledPicture-sc-1s3zfhk-0 hwMBxB']");
                        if (imageBlock != null)
                        {
                            var img = await imageBlock.QuerySelectorAsync("//img");
                            if (img != null)
                            {
                                image = await img.GetAttributeAsync("src");
                            }
                        }

                        product.ProductImageUrl = image;

                        //Getting the Url to the product details page
                        var urlBLock = await element.QuerySelectorAsync("//a[@class='Linkstyled__StyledLinkRouter-sc-1drhx1h-2 dqwdXM ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV']");
                        if (urlBLock == null) continue;

                        var url = await urlBLock.GetAttributeAsync("href");

                        if (string.IsNullOrEmpty(url)) continue;

                        product.ProductUrl = url;

                        var priceBlock = await element.QuerySelectorAsync("//span[@aria-hidden='true']");
                        if (priceBlock == null) continue;

                        var priceSpans = await priceBlock.QuerySelectorAllAsync("//*");

                        if (!priceSpans.Any()) continue;

                        var priceEuro = await priceSpans[0].TextContentAsync();
                        if (priceEuro == null) continue;

                        priceEuro = priceEuro.Replace(".", "");
                        if (!float.TryParse(priceEuro, out var euroResult)) continue;

                        switch (priceSpans.Count)
                        {
                            case 2:
                            {
                                var priceCent = await priceSpans[1].TextContentAsync();
                                if (priceCent == null) continue;

                                if (!float.TryParse(priceCent, out var centResult)) continue;

                                product.Price = euroResult + (centResult / 100);
                                break;
                            }
                            case 1:
                                product.Price = euroResult;
                                break;
                            default: continue;
                        }

                        product.Category = category;

                        product.Website = Website.Saturn;

                        var now = DateTime.Now;
                        product.LastUpdate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

                        products.Add(product);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                try
                {
                    pageNumber++;

                    var nextPage = await page.GotoAsync(categoryUrl + pageNumber);

                    if (nextPage == null || !nextPage.Ok) continue;

                    loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

                    Thread.Sleep(2500);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            await page.CloseAsync();

            return products;
        }
    }
}
