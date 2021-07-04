using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceIt.Data.Models;

namespace PriceIt.Models
{
    public class AddProductToListViewModel
    {
        public int ProductId { get; set; }
        public List<UserList> Lists { get; set; }
    }
}
