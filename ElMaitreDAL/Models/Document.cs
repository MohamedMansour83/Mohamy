using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Document")]
    public class Document : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FromUserId { get; set; }

        public string ToUserId { get; set; }

        public virtual ApplicationUser FromUser { get; set; }
        public virtual ApplicationUser ToUser { get; set; }

    }

}
