using System;
using System.Collections.Generic;
using System.Text;

namespace PriceIt.Data.Models
{
    public class ListItem
    {
        public int ListItemId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
