using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PriceIt.Core.Models;

namespace PriceIt.Core.Interfaces
{
    public interface IWebScraping
    {
        Task<List<Product>> GetAmazonProducts();
        Task<HtmlDocument> test();
        Task<HtmlDocument> HandelCaptcha(string amzn, string amznr, string captcha);
        Task<List<Product>> GetMediaMarktProducts();
    }
}
