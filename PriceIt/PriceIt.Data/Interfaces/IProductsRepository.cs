using System;
using System.Collections.Generic;
using System.Text;
using PriceIt.Core.Models;

namespace PriceIt.Data.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);

        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);

        bool ProductExists(int id);

        bool Save();
    }
}
