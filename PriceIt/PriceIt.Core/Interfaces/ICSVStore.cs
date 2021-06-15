using System.Collections.Generic;
using PriceIt.Data.Models;

namespace PriceIt.Core.Interfaces
{
    public interface ICSVStore
    {
        bool StoreProduct(Product product);
        void StoreProducts(List<Product> products);
        List<Product> ReadProducts();
    }
}
