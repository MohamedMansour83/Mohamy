using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Models
{
    public class DeliveryDetailsModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string BuildingNumber { get; set; }
        [Required]
        public string Floor { get; set; }
        [Required]
        public string Apartment { get; set; }
        [Required]
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}
