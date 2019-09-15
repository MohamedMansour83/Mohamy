using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Models
{
    [Table("Country")]
    public class Country : BaseModel
    {
        public string Name { get; set; }

        public int? AreaId { get; set; }

        public virtual Area Area { get; set; }

        public virtual ICollection<Province> Provinces { get; set; }

    }
}
