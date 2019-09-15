using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Review")]
    public class Review : BaseModel
    {
        public string Title { get; set; }
        public int Rate { get; set; }

        public int LawyerId { get; set; }
        public string UserId { get; set; }

        public virtual Lawyer Lawyer { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ReviewReply> Replies { get; set; }

    }
}
