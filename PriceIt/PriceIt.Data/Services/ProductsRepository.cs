using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceIt.Core.Models;
using PriceIt.Data.DbContexts;
using PriceIt.Data.Interfaces;

namespace PriceIt.Data.Services
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public IEnumerable<Product> GetProducts()
        {
            return _appDbContext.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            if (id < 1)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _appDbContext.Products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _appDbContext.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            //
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _appDbContext.Products.Remove(product);
        }

        public bool ProductExists(int id)
        {
            if (id < 1)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _appDbContext.Products.Any(p => p.Id == id);
        }

        public bool Save()
        {
            return (_appDbContext.SaveChanges() >= 0);
        }
    }
}
