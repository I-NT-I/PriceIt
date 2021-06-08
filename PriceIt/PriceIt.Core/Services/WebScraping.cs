using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("PriceItDevTest_3");

            var html = await httpClient.GetStringAsync(BaseUrl_Amazon);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var website = new HtmlWeb {AutoDetectEncoding = false, OverrideEncoding = Encoding.Default,UseCookies = true};
            var doc = await website.LoadFromWebAsync(BaseUrl_Amazon);

            var links = htmlDocument.DocumentNode.Descendants("a").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("a-link-normal a-text-normal"));

            foreach (var link in links)
            {
                var url = "https://www.amazon.de/" + link.Attributes["href"].Value;

                var name = "";

                try
                {
                    name = link.Descendants("span").Where(d =>
                        d.Attributes.Contains("class") && d.Attributes["class"].Value
                            .Contains("a-size-base-plus a-color-base a-text-normal")).ElementAt(0).InnerText;
                }
                catch (Exception e)
                {
                    
                }

                products.Add(new Product()
                {
                    Name = name,
                    ProductUrl = url
                });
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

            //var countBlock = pageOne.DocumentNode.Descendants("h1").FirstOrDefault(d =>
            //    d.Attributes.Contains("class") && d.Attributes["class"].Value
            //        .Contains("Typostyled__StyledInfoTypo-sc-1jga2g7-0 csVJPs"));

            //if (countBlock != null && int.TryParse(countBlock.FirstChild.InnerText.Substring(0, 4), out var count))
            //{
            //    var currentCount = 0;

            //    while (currentCount < count)
            //    {
            //        var page = await _callManager.CallWebsite("de/search.html?query=computer%20netzteil&page=" + currentCount ,"https://www.mediamarkt.de/");

            //        var pageObjs = page.DocumentNode.Descendants("div").Where(d =>
            //            d.Attributes.Contains("class") && d.Attributes["class"].Value
            //                .Contains("ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw"));

            //        foreach (var obj in pageObjs)
            //        {
            //            var product = new Product();

            //            var NameBlock = obj.Descendants("p").FirstOrDefault(d =>
            //                d.Attributes.Contains("class") && d.Attributes["class"].Value
            //                    .Contains("Typostyled__StyledInfoTypo-sc-1jga2g7-0 iLnJNN"));

            //            if (NameBlock != null)
            //            {
            //                var name = "";

            //                foreach (var child in NameBlock.ChildNodes)
            //                {
            //                    name += child.InnerText;
            //                }

            //                if (!string.IsNullOrEmpty(name))
            //                {
            //                    product.Name = name;
            //                }
            //            }

            //            var priceBlock = obj.Descendants("span").FirstOrDefault(d => d.Attributes.Contains("aria-hidden"));

            //            if (priceBlock != null)
            //            {
            //                var priceBlockEuro = priceBlock.ChildNodes[0];

            //                if (priceBlockEuro != null)
            //                {
            //                    if (float.TryParse(priceBlockEuro.InnerText.Replace(".", ""), out var priceEuro))
            //                    {
            //                        var priceBlockCent = priceBlock.ChildNodes[1];

            //                        if (priceBlockCent != null)
            //                        {
            //                            if (float.TryParse(priceBlockCent.InnerText, out var priceCent))
            //                            {
            //                                product.Price = priceEuro + (priceCent / 100);
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //            var linkBLock = obj.Descendants("a").FirstOrDefault(d =>
            //                d.Attributes.Contains("class") && d.Attributes["class"].Value
            //                    .Contains("Linkstyled__StyledLinkRouter-sc-1drhx1h-2 hihJjl ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV"));

            //            if (linkBLock != null)
            //            {
            //                var link = "";

            //                if (linkBLock.HasAttributes && linkBLock.Attributes.Contains("href"))
            //                {
            //                    link = linkBLock.Attributes["href"].Value;
            //                }

            //                if (!string.IsNullOrEmpty(link))
            //                {
            //                    product.ProductUrl = "https://www.mediamarkt.de" + link;
            //                }
            //            }

            //            product.Website = Website.MediaMarkt;

            //            products.Add(product);

            //            currentCount++;
            //        }

            //        _csvStore.StoreProducts(products);
            //        products = new List<Product>();
            //    }
            //}

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
