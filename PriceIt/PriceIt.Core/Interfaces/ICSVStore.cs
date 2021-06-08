using System;
using System.Collections.Generic;
using System.Text;
using PriceIt.Core.Models;

namespace PriceIt.Core.Interfaces
{
    public interface ICSVStore
    {
        bool StoreProduct(Product product);
        void StoreProducts(List<Product> products);
        List<Product> ReadProducts();
    }
}
