using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PriceIt.Core.Models;
using PriceIt.Data.Models;

namespace PriceIt.Core.Interfaces
{
    public interface IWebScraping
    {
        Task ScrapAllWebSites();
        Task GetAmazonProducts();
        Task GetMediaMarktProducts();
        Task GetSaturnProducts();
    }
}
