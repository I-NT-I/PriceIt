using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PriceIt.Core.Models;

namespace PriceIt.Core.Interfaces
{
    public interface IWebScraping
    {
        Task<List<Product>> GetAmazonProducts();
        Task<List<Product>> GetMediaMarktProducts();
        Task<List<Product>> GetSaturnProducts();
    }
}
