using System;
using System.Collections.Generic;
using System.Text;

namespace PriceIt.Data.Models
{
    public class ListItem
    {
        public int ListItemId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public int UserListId { get; set; }
        public virtual UserList UserList { get; set; }
    }
}
