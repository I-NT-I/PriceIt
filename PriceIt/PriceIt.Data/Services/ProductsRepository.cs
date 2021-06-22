using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            if (!string.IsNullOrEmpty(product.ProductIdentifier))
            {
                var oldItem =
                    _appDbContext.Products.FirstOrDefault(p => p.ProductIdentifier == product.ProductIdentifier);

                if (oldItem == null)
                {
                    _appDbContext.Products.Add(product);
                }
                else
                {
                    _appDbContext.Entry(oldItem).State = EntityState.Detached;
                    var entity = _appDbContext.Products.Attach(product);
                    entity.State = EntityState.Modified;
                }
            }
            else
            {
                var oldItem =
                    _appDbContext.Products.FirstOrDefault(p => p.ProductUrl == product.ProductUrl);

                if (oldItem == null)
                {
                    _appDbContext.Products.Add(product);
                }
                else
                {
                    _appDbContext.Entry(oldItem).State = EntityState.Detached;
                    var entity = _appDbContext.Products.Attach(product);
                    entity.State = EntityState.Modified;
                }
            }
        }

        public async Task AddProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (!string.IsNullOrEmpty(product.ProductIdentifier))
            {
                var oldItem =
                    await _appDbContext.Products.FirstOrDefaultAsync(p => p.ProductIdentifier == product.ProductIdentifier);

                if (oldItem == null)
                {
                    await _appDbContext.Products.AddAsync(product);
                }
                else
                {
                    _appDbContext.Entry(oldItem).State = EntityState.Detached;
                    var entity = _appDbContext.Products.Attach(product);
                    entity.State = EntityState.Modified;
                }
            }
            else
            {
                var oldItem =
                    await _appDbContext.Products.FirstOrDefaultAsync(p => p.ProductUrl == product.ProductUrl);

                if (oldItem == null)
                {
                    await _appDbContext.Products.AddAsync(product);
                }
                else
                {
                    _appDbContext.Entry(oldItem).State = EntityState.Detached;
                    var entity = _appDbContext.Products.Attach(product);
                    entity.State = EntityState.Modified;
                }
            }
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var entity = _appDbContext.Products.Attach(product);
            entity.State = EntityState.Modified;
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _appDbContext.Products.Remove(product);
        }

        public async Task<List<Product>> Search(string query)
        {
            var products = new List<Product>();

            if (!string.IsNullOrEmpty(query))
            {
                products = await _appDbContext.Products.Where(p => p.Name.Contains(query)).ToListAsync();
            }

            return products;
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
