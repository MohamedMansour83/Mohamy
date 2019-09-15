using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("LawyerAppointment")]
    public class LawyerAppointment : BaseModel
    {
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int LawyerId { get; set; }
        public virtual Lawyer Lawyer { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }


    }



}
