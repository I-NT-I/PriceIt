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
        public string Image { get; set; }
        public Category Category { get; set; }
    }

    public enum Website
    {
        Amazon = 1,
        Saturn = 2,
        MediaMarkt = 3
    }

    public enum Category
    {
        MotherBoard = 1,
        CPU = 2,
        RAM = 3,
        PowerSupply = 4,
        GraphicCard = 5,
        Storge = 6
    }
}
