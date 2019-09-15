using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.ViewModels
{
    public class RegisterModel
    {
        [Display(Name = "User Name")]
        [StringLength(50, ErrorMessage = "Invalid {0}")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "{0} is not valid.")]
        [StringLength(128, ErrorMessage = "{0} is not valid.")]
        [Required]
        public string Email { get; set; }



        [RegularExpression("^(01)[0-9]{9}$", ErrorMessage = "{0} is not valid.")]
        [Required]
        public string PhoneNumber { get; set; }


        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8,ErrorMessage ="Invalid {0}")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "The password and confirmation password do not match.")]
        [Required()]
        public string ConfirmPassword { get; set; }

        [Required()]
        public Gender Gender { get; set; }

        public bool IsLawyer { get; set; }

        public string FbId { get; set; }
    }
}
