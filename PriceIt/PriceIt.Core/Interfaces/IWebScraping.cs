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
        Task GetAmazonProducts();
        Task<List<Product>> GetMediaMarktProducts();
        Task<List<Product>> GetSaturnProducts();
    }
}
