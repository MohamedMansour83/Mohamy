using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Models
{
    [Table("LawyerService")]
    public class LawyerService : BaseModel
    {
        public int ServiceId { get; set; }
        public int LawyerId { get; set; }
        public int PriceProvided { get; set; }
        public int PriceLevel2Provided { get; set; }

        public virtual Service Service { get; set; }
        public virtual Lawyer Lawyer { get; set; }


    }
}
