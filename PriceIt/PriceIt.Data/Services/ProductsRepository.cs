using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceIt.Data.DbContexts;
using PriceIt.Data.Interfaces;
using PriceIt.Data.Models;

namespace PriceIt.Data.Services
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _appDbContext.Products.ToListAsync();
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

            var checkIfExists = _appDbContext.Products.FirstOrDefault(x => x.ProductUrl == product.ProductUrl);

            if (checkIfExists == null)
            {
                _appDbContext.Products.Add(product);
            }
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var checkIfExists = _appDbContext.Products.FirstOrDefault(x => x.ProductUrl == product.ProductUrl);

            if (checkIfExists == null)
            {
                throw new Exception("doesn't exists");
            }

            product.Id = checkIfExists.Id;
            _appDbContext.Products.Update(product);
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
