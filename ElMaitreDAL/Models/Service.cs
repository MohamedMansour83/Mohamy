using ElMaitre.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Models
{
    [Table("Service")]
    public class Service : BaseModel
    {
        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string ContactNumber { get; set; }
        public string PaperWork { get; set; }
        public int Duration { get; set; }
        public int CategoryId { get; set; }
        public virtual ServiceCategory Category { get; set; }
        public virtual ICollection<ServicePrice> Prices { get; set; }
        public virtual ICollection<LawyerService> Lawyers { get; set; }
    }

}
