using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceIt.Data.Models;

namespace PriceIt.Models
{
    public class ListViewModel
    {
        public UserList List { get; set; }
        public List<ListItem> Items { get; set; }
    }
}
