using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceIt.Data.Models;

namespace PriceIt.Models
{
    public class SearchResultViewModel
    {
        public string Query { get; set; }
        public Website Website { get; set; }
        public Category Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
