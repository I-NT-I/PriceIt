﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Product>> GetAmazonProducts()
        {
            var products = new List<Product>();

            var website = new HtmlWeb {AutoDetectEncoding = false, OverrideEncoding = Encoding.Default,UseCookies = true};
            var doc = await website.LoadFromWebAsync(BaseUrl_Amazon);

            var links = doc.DocumentNode.Descendants("a").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("a-link-normal a-text-normal"));

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
    }
}
