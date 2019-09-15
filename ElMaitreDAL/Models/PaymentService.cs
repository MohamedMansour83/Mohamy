using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElMaitre.DAL.Models
{
    [Table("PaymentService")]
    public class PaymentService : BaseModel
    {
        public int ServiceId { get; set; }
        public int LawyerId { get; set; }
        public long OrderId { get; set; }
        public string UserId { get; set; }
        public bool IsPaymentConfirmed { get; set; }

        public virtual Service Service { get; set; }
        public virtual Lawyer Lawyer { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}
