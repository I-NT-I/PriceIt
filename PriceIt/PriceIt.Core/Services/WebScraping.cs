using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PriceIt.Core.Interfaces;
using PriceIt.Core.Models;

namespace PriceIt.Core.Services
{
    public class WebScraping : IWebScraping
    {
        private readonly string BaseUrl_Amazon = "https://www.amazon.de/s?bbn=340843031&rh=n%3A17453196031&brr=1&rd=1&ref=Oct_s9_apbd_odnav_hd_bw_bSxeud_0";

        private readonly IHttpCallManager _callManager;

        public WebScraping(IHttpCallManager callManager)
        {
            _callManager = callManager;
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
            var products = new List<Product>();

            var PageOne = await _callManager.CallWebsite("de/search.html?query=computer%20netzteil&t=1621809809259&user_input=computer%20ne&query_from_suggest=true", "https://www.mediamarkt.de/");
            var objs = PageOne.DocumentNode.Descendants("div").Where(d =>
                d.Attributes.Contains("class") && d.Attributes["class"].Value
                    .Contains("ProductFlexBox__StyledListItem-nk9z2u-0 kzcilw"));

            foreach (var obj in objs)
            {
                var product = new Product();

                var NameBlock = obj.Descendants("p").FirstOrDefault(d =>
                    d.Attributes.Contains("class") && d.Attributes["class"].Value
                        .Contains("Typostyled__StyledInfoTypo-sc-1jga2g7-0 iLnJNN"));

                if (NameBlock != null)
                {
                    var name = "";

                    foreach (var child in NameBlock.ChildNodes)
                    {
                        name += child.InnerText;
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        product.Name = name;
                    }
                }

                var priceBlock = obj.Descendants("span").FirstOrDefault(d => d.Attributes.Contains("aria-hidden"));

                if (priceBlock != null)
                {
                    var priceBlockEuro = priceBlock.ChildNodes[0];

                    if (priceBlockEuro != null)
                    {
                        if (float.TryParse(priceBlockEuro.InnerText.Replace(".",""), out var priceEuro))
                        {
                            var priceBlockCent = priceBlock.ChildNodes[1];

                            if (priceBlockCent != null)
                            {
                                if (float.TryParse(priceBlockCent.InnerText, out var priceCent))
                                { 
                                    product.Price = priceEuro + (priceCent / 100);
                                }
                            }
                        }
                    }
                }

                var linkBLock = obj.Descendants("a").FirstOrDefault(d =>
                    d.Attributes.Contains("class") && d.Attributes["class"].Value
                        .Contains("Linkstyled__StyledLinkRouter-sc-1drhx1h-2 hihJjl ProductListItemstyled__StyledLink-sc-16qx04k-0 dYJAjV"));

                if (linkBLock != null)
                {
                    var link = "";

                    if (linkBLock.HasAttributes && linkBLock.Attributes.Contains("href"))
                    {
                        link = linkBLock.Attributes["href"].Value;
                    }

                    if (!string.IsNullOrEmpty(link))
                    {
                        product.ProductUrl = "https://www.mediamarkt.de" + link;
                    }
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
