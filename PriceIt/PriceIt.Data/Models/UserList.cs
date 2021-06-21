using System;
using System.Collections.Generic;
using System.Text;

namespace PriceIt.Data.Models
{
    public class UserList
    {
        public int UserListId { get; set; }
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual AppUser User { get; set; }
        public string UserId { get; set; }
    }
}
