using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PriceIt.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A Product must have a name")]
        [StringLength(250, ErrorMessage = "The name must not be more than 250 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Product must have a price")]
        public float Price { get; set; }

        [Required(ErrorMessage = "A Product must have a URL")]
        [StringLength(500, ErrorMessage = "The Product URL must not be more than 500 characters long")]
        public string ProductUrl { get; set; }

        [StringLength(500, ErrorMessage = "The Product Image URL must not be more than 500 characters long")]
        public string ProductImageUrl { get; set; }

        [Required(ErrorMessage = "A Product must have a Website")]
        public Website Website { get; set; }
        public Category Category { get; set; }

        public DateTime LastUpdate { get; set; }
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
