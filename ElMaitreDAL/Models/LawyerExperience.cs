using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("LawyerExperience")]
    public class LawyerExperience : BaseModel
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int Price { get; set; }

        public virtual ICollection<Lawyer> Lawyers { get; set; }

    }

}
