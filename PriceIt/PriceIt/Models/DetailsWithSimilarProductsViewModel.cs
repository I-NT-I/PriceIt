using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceIt.Data.Models;

namespace PriceIt.Models
{
    public class DetailsWithSimilarProductsViewModel
    {
        public Product Product { get; set; }
        public List<Product> SimilarProducts { get; set; }
    }
}
