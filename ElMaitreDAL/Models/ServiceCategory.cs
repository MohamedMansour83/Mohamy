using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("ServiceCategory")]
    public class ServiceCategory : BaseModel
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string IconPath { get; set; }

        public virtual ICollection<Service> Services { get; set; }

    }

}
