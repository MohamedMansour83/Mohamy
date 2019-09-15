using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("PriceRange")]
    public class PriceRange : BaseModel
    {
        public double From { get; set; }

        public double To { get; set; }
    }

}
