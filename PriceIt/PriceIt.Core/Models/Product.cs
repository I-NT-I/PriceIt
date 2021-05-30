using System;
using System.Collections.Generic;
using System.Text;

namespace PriceIt.Core.Models
{
    public class Product
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string ProductUrl { get; set; }
        public Website Website { get; set; }
    }

    public enum Website
    {
        Amazon = 1,
        Saturn = 2,
        MediaMarkt = 3
    }
}
