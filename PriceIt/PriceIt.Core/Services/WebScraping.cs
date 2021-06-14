using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PriceIt.Core.Interfaces;
using PriceIt.Core.Models;

namespace PriceIt.Core.Services
{
    public class WebScraping : IWebScraping
    {
        private const string BaseUrlAmazon = "https://www.amazon.de/";
        private const string BaseUrlMediaMarkt = "https://www.mediamarkt.de";
        private const string BaseUrlSaturn = "https://www.saturn.de";

            private readonly int _pagesToScrap = 5;

        private readonly IHttpCallManager _callManager;
        private readonly ICSVStore _csvStore;

        public WebScraping(IHttpCallManager callManager, ICSVStore csvStore)
        {
            _callManager = callManager;
            _csvStore = csvStore;
        }

        public async Task<List<Product>> GetAmazonProducts()
        {
            var products = new List<Product>();

            using var playwright = await Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome" ,Headless = false});

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://www.amazon.de/b/?ie=UTF8&node=430177031&pf_rd_p=2460ece3-989d-411e-b2fb-3f40528cf506&pf_rd_r=E3X5BXZBEVWQ029HVY7H&pf_rd_s=visualsn_de_pc-content-6&pf_rd_t=SubnavFlyout&ref_=sn_gfs_co_computervs_430177031_6");

            var nextPageBlock = await page.QuerySelectorAsync("//li[@class='a-last']");
            var pageNumber = 0;

            while (nextPageBlock != null && pageNumber < _pagesToScrap)
            {
                var elements = await page.QuerySelectorAllAsync(
                    "//div[@class='s-include-content-margin s-border-bottom s-latency-cf-section']");

                foreach (var element in elements)
                {
                    var product = new Product();

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

                    product.Image = image;

                    //Getting the Url to the product details page
                    var urlBLock = await element.QuerySelectorAsync("//a[@class='a-link-normal s-no-outline']");
                    if (urlBLock == null) continue;

                    var url = await urlBLock.GetAttributeAsync("href");

                    if (string.IsNullOrEmpty(url)) continue;

                    product.ProductUrl = BaseUrlAmazon + url;

                    //Getting the price of the product
                    var priceBlock = await element.QuerySelectorAsync("//span[@class='a-price-whole']");
                    if (priceBlock == null) continue;

                    var priceValue = await priceBlock.TextContentAsync();

                    if (string.IsNullOrEmpty(priceValue)) continue;
                    
                    if (!float.TryParse(priceValue, out var result)) continue;

                    product.Price = result;

                    product.Category = Category.CPU;

                    product.Website = Website.Amazon;

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

        public async Task<List<Product>> GetMediaMarktProducts()
        {
            var products = new List<Product>();

            using var playwright = await Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = false });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var pageNumber = 1;

            await page.GotoAsync("https://www.mediamarkt.de/de/category/_arbeitsspeicher-ram-462907.html?page="+ pageNumber);

            var loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

            while (loadMoreBlock != null && pageNumber <= _pagesToScrap)
            {
                var elements = await page.QuerySelectorAllAsync("//div[@class='ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw']");

                foreach (var element in elements)
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

                    product.Image = image;

                    //Getting the Url to the product details page
                    var urlBLock = await element.QuerySelectorAsync("//a[@class='Linkstyled__StyledLinkRouter-sc-1drhx1h-2 iDDAGF ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV']");
                    if (urlBLock == null) continue;

                    var url = await urlBLock.GetAttributeAsync("href");

                    if (string.IsNullOrEmpty(url)) continue;

                    product.ProductUrl = BaseUrlMediaMarkt + url;

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

                    product.Category = Category.RAM;

                    product.Website = Website.MediaMarkt;

                    products.Add(product);
                }

                pageNumber++;

                var nextPage = await page.GotoAsync("https://www.mediamarkt.de/de/category/_arbeitsspeicher-ram-462907.html?page=" + pageNumber);

                if (nextPage == null || !nextPage.Ok) continue;

                loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

                Thread.Sleep(2500);
            }

            await page.CloseAsync();

            return products;
        }

        public async Task<List<Product>> GetSaturnProducts()
        {
            var products = new List<Product>();

            using var playwright = await Playwright.CreateAsync();
            var chromium = playwright.Chromium;
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = false });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var pageNumber = 1;

            await page.GotoAsync("https://www.saturn.de/de/category/_grafikkarten-286896.html?page=" + pageNumber);

            var loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

            while (loadMoreBlock != null && pageNumber <= _pagesToScrap)
            {
                var elements =
                    await page.QuerySelectorAllAsync("//div[@class='ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw']");

                foreach (var element in elements)
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

                    product.Image = image;

                    //Getting the Url to the product details page
                    var urlBLock = await element.QuerySelectorAsync("//a[@class='Linkstyled__StyledLinkRouter-sc-1drhx1h-2 dqwdXM ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV']");
                    if (urlBLock == null) continue;

                    var url = await urlBLock.GetAttributeAsync("href");

                    if (string.IsNullOrEmpty(url)) continue;

                    product.ProductUrl = BaseUrlSaturn + url;

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

                    product.Category = Category.GraphicCard;

                    product.Website = Website.Saturn;

                    products.Add(product);
                }

                pageNumber++;

                var nextPage = await page.GotoAsync("https://www.saturn.de/de/category/_grafikkarten-286896.html?page=" + pageNumber);

                if(nextPage == null || !nextPage.Ok) continue;

                loadMoreBlock = await page.QuerySelectorAsync("//button[@data-test='mms-search-srp-loadmore']");

                Thread.Sleep(2500);
            }

            await page.CloseAsync();

            return products;
        }

        public async Task<HtmlDocument> test()
        {

            var amazonPageOne = await _callManager.CallWebsite("de/search.html?query=computer%20netzteil&t=1621809809259&user_input=computer%20ne&query_from_suggest=true", "https://www.mediamarkt.de/");

            var searchResultNumber = amazonPageOne.DocumentNode.Descendants("h1").Where(d =>
                d.Attributes.Contains("class") && d.Attributes["class"].Value
                    .Contains("Typostyled__StyledInfoTypo-sc-1jga2g7-0 csVJPs"));

            var doc = new HtmlDocument();

            doc.DocumentNode.AppendChild(searchResultNumber.FirstOrDefault());

            return doc;
        }

        public async Task<HtmlDocument> HandelCaptcha(string amzn,string amznr, string captcha)
        {
            var uri = new Uri("https://www.amazon.de/errors/validateCaptcha", UriKind.Absolute);
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("amzn", amzn));
            formVariables.Add(new KeyValuePair<string, string>("amzn-r", amznr));
            formVariables.Add(new KeyValuePair<string, string>("field-keywords", captcha));

            var formContent = new FormUrlEncodedContent(formVariables);
            
            var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.TryParseAdd("PriceItDevTest_2");

            using (var message = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = uri, Content = formContent })
            {
                // use HttpClient to send the message
                using (var postResponse = await client.SendAsync(message))
                {
                    if (postResponse.IsSuccessStatusCode)
                    {
                        var stringContent = await postResponse.Content.ReadAsStringAsync();

                        var doc = new HtmlDocument();
                        doc.LoadHtml(stringContent);

                        return doc;
                    }
                }
            }

            return null;
        }
    }
}
