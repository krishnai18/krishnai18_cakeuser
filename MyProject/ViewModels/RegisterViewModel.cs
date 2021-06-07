using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string FullName{ get; set; }
        [Required]

        [DataType(DataType.Date)]

        public DateTime Birthdate { get; set; }





    }
}
