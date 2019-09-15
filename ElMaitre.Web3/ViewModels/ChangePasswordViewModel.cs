using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class ChangePasswordViewModel
    {

        public string OldPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Invalid {0}")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "The password and confirmation password do not match.")]
        [Required()]
        public string ConfirmPassword { get; set; }
    }
}
