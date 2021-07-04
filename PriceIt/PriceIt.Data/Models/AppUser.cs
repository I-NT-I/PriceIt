using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.CompilerServices;

namespace PriceIt.Data.Models
{
    public class AppUser : IdentityUser
    {
        public List<UserList> UserLists { get; set; }
    }
}
