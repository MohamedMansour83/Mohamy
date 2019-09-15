using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("ReviewReply")]
    public class ReviewReply : BaseModel
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public int ReviewId { get; set; }



        public virtual Review Review { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
