using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PriceIt.Core.Models;

namespace PriceIt.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product(){ Id = 1,Name = "TestProduct1",Price = 60.99f,ProductUrl = "https://www.saturn.de/de/product/_cooler-master-masterwatt-lite-700w-230v-2602309.html", ProductImageUrl = "https://assets.mmsrg.com/isr/166325/c1/-/ASSET_MMS_72715345/fee_786_587_png" ,LastUpdate = DateTime.Now},
                new Product() { Id = 2, Name = "TestProduct2", Price = 60.99f, ProductUrl = "https://www.saturn.de/de/product/_seasonic-core-gc-650-gold-2625115.html", ProductImageUrl = "https://assets.mmsrg.com/isr/166325/c1/-/ASSET_MMS_72796414/fee_786_587_png" , LastUpdate = DateTime.Now },
                new Product() { Id = 3, Name = "TestProduct3", Price = 60.99f, ProductUrl = "https://www.saturn.de/de/product/_be-quiet-pure-power-11-500w-2505555.html", ProductImageUrl = "https://assets.mmsrg.com/isr/166325/c1/-/pixelboxx-mss-80864028/fee_786_587_png", LastUpdate = DateTime.Now });
            base.OnModelCreating(modelBuilder);
        }
    }
}
