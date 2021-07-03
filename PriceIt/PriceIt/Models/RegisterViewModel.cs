using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PriceIt.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please provide an email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please repeat the password")]
        [DataType(DataType.Password)]
        public string PasswordCheck { get; set; }
    }
}
