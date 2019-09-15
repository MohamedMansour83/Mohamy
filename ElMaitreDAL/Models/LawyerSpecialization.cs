using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("LawyerSpecialization")]
    public class LawyerSpecialization : BaseModel
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public virtual ICollection<Lawyer> Lawyers { get; set; }
    }
}
