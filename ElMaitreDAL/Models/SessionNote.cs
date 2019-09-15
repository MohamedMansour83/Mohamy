using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("SessionNote")]
    public class SessionNote : BaseModel
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public int SessionId { get; set; }
        public virtual Session Session { get; set; }
        public virtual ApplicationUser User { get; set; }

    }

}
