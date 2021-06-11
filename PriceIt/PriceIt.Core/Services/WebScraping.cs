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
        private readonly string BaseUrl_Amazon = "https://www.amazon.de/s?bbn=340843031&rh=n%3A17453196031&brr=1&rd=1&ref=Oct_s9_apbd_odnav_hd_bw_bSxeud_0";

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
            var browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome" });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://www.amazon.de/-/en/s?k=pc+netzteil&crid=VB0L1QCD2ESY&qid=1623326023&sprefix=pc+net%2Caps%2C174&ref=sr_pg_1");

            var elements = await page.QuerySelectorAllAsync(
                "//div[@class='s-include-content-margin s-border-bottom s-latency-cf-section']");

            foreach (var element in elements)
            {
                var product = new Product();


                //Getting the name of the product
                var nameBlock = await element.QuerySelectorAsync("//span[@class='a-size-medium a-color-base a-text-normal']");
                if (nameBlock == null) continue;


                var name = await nameBlock.TextContentAsync();
                if (!string.IsNullOrEmpty(name))
                {
                    product.Name = name;
                }

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

                if (!string.IsNullOrEmpty(image))
                {
                    product.Image = image;
                }

                //Getting the Url to the product details page
                var url = "";
                var urlBLock = await element.QuerySelectorAsync("//a[@class='a-link-normal s-no-outline']");
                if(urlBLock == null) continue;

                url = await urlBLock.GetAttributeAsync("href");

                if (!string.IsNullOrEmpty(url))
                {
                    product.ProductUrl = url;
                }

                //Getting the price of the product
                var priceBlock = await element.QuerySelectorAsync("//span[@class='a-price-whole']");
                if(priceBlock == null) continue;

                var priceValue = await priceBlock.TextContentAsync();

                if (!string.IsNullOrEmpty(priceValue))
                {
                    var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    culture.NumberFormat.NumberDecimalSeparator = ".";
                    product.Price = float.Parse(priceValue,culture);
                }

                product.Website = Website.Amazon;

                products.Add(product);
            }

            return products;
        }

        public async Task<List<Product>> GetMediaMarktProducts()
        {
            IWebDriver driver = new ChromeDriver(@"D:\WorkSpace\Git\PriceIt\PriceIt\PriceIt.Core\bin\Debug\netcoreapp3.1");
            driver.Url = "https://www.mediamarkt.de/de/search.html?query=computer%20netzteil&t=1621809809259&user_input=computer%20ne&query_from_suggest=true";

            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(10000));

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            ReadOnlyCollection<IWebElement> elements = driver.FindElements(
                By.XPath("//div[@class='ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw']"));

            var step = elements[0].Size.Height;
            Int64 last_height = (Int64)0;
            Int64 max_height = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight");
            while (true)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, "+last_height+");");
                /* Wait to load page */
                Thread.Sleep(2000);
                /* Calculate new scroll height and compare with last scroll height */
                var new_height = last_height + step;
                if (new_height >= max_height)
                    /* If heights are the same it will exit the function */
                    break;
                last_height = new_height;
            }


            driver.Close();
            //var test = _csvStore.ReadProducts();

            var products = new List<Product>();

            foreach (var element in elements)
            {
                var product = new Product();

                var image = "";

                try
                {
                    var imageBlock =
                        element.FindElement(
                            By.XPath(".//div[@class='ProductImage__StyledPictureWrapper-sc-11s4esr-0 dYgeHb']"));

                    image = imageBlock.FindElement(By.TagName("img")).GetAttribute("src");
                }
                catch (Exception e)
                {
                    // ignored
                }

                product.Image = image;

                var nameBlockParent =
                    element.FindElement(
                        By.XPath(".//div[@class='ProductHeader__StyledHeadingWrapper-cwyxax-0 fGyNfx']"));

                var nameBlock = nameBlockParent.FindElement(By.TagName("p"));

                var nameSpan = nameBlock.FindElement(By.TagName("span"));

                var name = nameSpan.GetAttribute("innerText") + nameBlock.GetAttribute("innerText");

                if (!string.IsNullOrEmpty(name))
                {
                    product.Name = name;
                }

                var priceBlockParent =
                    element.FindElement(
                        By.XPath(".//span[@aria-hidden='true']"));

                var priceSpans = priceBlockParent.FindElements(By.CssSelector("*"));

                if (priceSpans != null)
                {
                    var priceBlockEuro = priceSpans[0];

                    var priceEuro = priceSpans[0].GetAttribute("innerText").Replace(".", "");

                    var priceCent = priceSpans[1].GetAttribute("innerText");

                    product.Price = float.Parse(priceEuro) + (float.Parse(priceCent) / 100);
                }

                var linkBLock = element.FindElement(By.XPath(".//a[@class='Linkstyled__StyledLinkRouter-sc-1drhx1h-2 hihJjl ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV']"));

                var link = linkBLock.GetAttribute("href");

                if (!string.IsNullOrEmpty(link))
                {
                    product.ProductUrl = link;
                }

                product.Website = Website.MediaMarkt;

                products.Add(product);
            }

            var pageOne = await _callManager.CallWebsite("de/search.html?query=computer%20netzteil&t=1621809809259&user_input=computer%20ne&query_from_suggest=true", "https://www.mediamarkt.de/");
            var objs = pageOne.DocumentNode.Descendants("div").Where(d =>
                d.Attributes.Contains("class") && d.Attributes["class"].Value
                    .Contains("ProductTilestyled__StyledCardWrapper-sc-1w38xrp-0 iQOLvF"));

            return products;
        }

        public async Task<List<Product>> GetSaturnProducts()
        {

            IWebDriver driver = new ChromeDriver(@"D:\WorkSpace\Git\PriceIt\PriceIt\PriceIt.Core\bin\Debug\netcoreapp3.1");
            driver.Url = "https://www.saturn.de/de/search.html?query=computer%20netzteil&t=1623073505284&user_input=computer%20netzteil&query_from_suggest=true";

            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(10000));

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            ReadOnlyCollection<IWebElement> elements = driver.FindElements(
                By.XPath("//div[@class='ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw']"));

            var step = elements[0].Size.Height;
            Int64 last_height = (Int64)0;
            Int64 max_height = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight");
            while (true)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, " + last_height + ");");
                /* Wait to load page */
                Thread.Sleep(2000);
                /* Calculate new scroll height and compare with last scroll height */
                var new_height = last_height + step;
                if (new_height >= max_height)
                    /* If heights are the same it will exit the function */
                    break;
                last_height = new_height;
            }

            var products = new List<Product>();

            foreach (var element in elements)
            {
                var product = new Product();

                var image = "";

                try
                {
                    var imageBlock =
                        element.FindElement(
                            By.XPath(".//div[@class='Picturestyled__StyledPicture-sc-1s3zfhk-0 hwMBxB']"));

                    image = imageBlock.FindElement(By.TagName("img")).GetAttribute("src");
                }
                catch (Exception e)
                {
                    // ignored
                }

                product.Image = image;

                var nameBlockParent =
                    element.FindElement(
                        By.XPath(".//div[@class='ProductHeader__StyledHeadingWrapper-cwyxax-0 fGyNfx']"));

                var nameBlock = nameBlockParent.FindElement(By.TagName("p"));

                var nameSpan = nameBlock.FindElement(By.TagName("span"));

                var name = nameSpan.GetAttribute("innerText") + nameBlock.GetAttribute("innerText");

                if (!string.IsNullOrEmpty(name))
                {
                    product.Name = name;
                }

                var priceBlockParent =
                    element.FindElement(
                        By.XPath(".//span[@aria-hidden='true']"));

                var priceSpans = priceBlockParent.FindElements(By.CssSelector("*"));

                if (priceSpans != null)
                {
                    var priceBlockEuro = priceSpans[0];

                    var priceEuro = priceSpans[0].GetAttribute("innerText").Replace(".", "");

                    var priceCent = priceSpans[1].GetAttribute("innerText");

                    product.Price = float.Parse(priceEuro) + (float.Parse(priceCent) / 100);
                }

                var linkBLock = element.FindElement(By.XPath(".//a[@class='Linkstyled__StyledLinkRouter-sc-1drhx1h-2 dqwdXM ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV']"));

                var link = linkBLock.GetAttribute("href");

                if (!string.IsNullOrEmpty(link))
                {
                    product.ProductUrl = link;
                }

                product.Website = Website.MediaMarkt;

                products.Add(product);
            }

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
