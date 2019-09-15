using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{

    [Table("Area")]
    public class Area : BaseModel
    {

        public string Name { get; set; }

        public virtual ICollection<Country> Countries { get; set; }

    }

}
