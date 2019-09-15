using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("QuestionAnswer")]
    public class QuestionAnswer : BaseModel
    {
        public string Title { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
