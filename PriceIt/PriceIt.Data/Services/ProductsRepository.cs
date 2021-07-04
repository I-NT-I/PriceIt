using System;
using System.Collections.Generic;
using System.Linq;
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
            return id < 1 ? null : _appDbContext.Products.FirstOrDefault(p => p.Id == id);
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

            try
            {
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
                        product.Id = oldItem.Id;
                        _appDbContext.Entry(oldItem).State = EntityState.Detached;
                        var entity = _appDbContext.Products.Attach(product);
                        entity.State = EntityState.Modified;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                var fuzzy = new Fuzzy(query);

                products = _appDbContext.Products.AsEnumerable().Where(p => fuzzy.IsMatch(p.Name)).ToList();
            }

            return products;
        }

        public List<Product> Search(string query, string website, List<string> categories)
        {
            List<Product> products;

            var fuzzy = new Fuzzy(query);

            var categoriesTag = new List<Category>();

            foreach (var category in categories)
            {
                if (Enum.TryParse<Category>(category, out var categoryTag))
                {
                    categoriesTag.Add(categoryTag);
                }
            }

            if (Enum.TryParse<Website>(website,out var websiteTag))
            {
                if (categoriesTag.Any())
                {
                    products = _appDbContext.Products?.AsEnumerable().Where(p =>
                        fuzzy.IsMatch(p.Name) && p.Website == websiteTag && categoriesTag.Contains(p.Category)).ToList();
                }
                else
                {
                    products = _appDbContext.Products?.AsEnumerable().Where(p =>
                        fuzzy.IsMatch(p.Name) && p.Website == websiteTag).ToList();
                }
            }
            else
            {
                if (categoriesTag.Any())
                {
                    products = _appDbContext.Products?.AsEnumerable().Where(p =>
                        fuzzy.IsMatch(p.Name) && categoriesTag.Contains(p.Category)).ToList();
                }
                else
                {
                    products = _appDbContext.Products?.AsEnumerable().Where(p =>
                        fuzzy.IsMatch(p.Name)).ToList();
                }
            }

            return products;
        }

        public List<Product> Search(string query, Category category)
        {
            var fuzzy = new Fuzzy(query);

            var products = _appDbContext.Products.AsEnumerable().Where(p => 
                p.Category == category).ToList();

            var simList = new List<SimProduct>();

            foreach (var product in products)
            {
                simList.Add(new SimProduct()
                {
                    SimValue = fuzzy.MatchRegexWord(product.Name),
                    Product = product
                });
            }

            simList = simList.OrderByDescending(s => s.SimValue).Take(10).ToList();

            return simList.Select(simProduct => simProduct.Product).ToList();
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
