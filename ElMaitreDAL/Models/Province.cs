using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Province")]
    public class Province : BaseModel
    {
        public string Name { get; set; }
        public string NameEn { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

    }



}
