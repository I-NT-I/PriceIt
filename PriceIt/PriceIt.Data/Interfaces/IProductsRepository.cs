using System.Collections.Generic;
using System.Threading.Tasks;
using PriceIt.Data.Models;

namespace PriceIt.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProducts();
        Product GetProduct(int id);

        void AddProduct(Product product);
        Task AddProductAsync(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        Task<List<Product>> Search(string query);
        List<Product> Search(string query, string website, List<string> categories);

        bool ProductExists(int id);

        bool Save();
    }
}
