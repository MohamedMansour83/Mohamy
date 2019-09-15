using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("ServicePrice")]
    public class ServicePrice : BaseModel
    {
        public double Price { get; set; }
        public int ServiceId { get; set; }
        public int ExperienceId { get; set; }

        public virtual Service Service { get; set; }
        public virtual LawyerExperience Experience { get; set; }


    }

}
